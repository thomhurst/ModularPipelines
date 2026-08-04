using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleConditionHandler : IModuleConditionHandler
{
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly IOptions<DistributedOptions> _distributedOptions;
    private readonly RoleDetector _roleDetector;
    private readonly IPipelineContextProvider _pipelineContextProvider;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly ConditionalWeakTable<IModule, ConditionEvaluation> _conditionEvaluations = new();
    private readonly ConcurrentDictionary<Type, Lazy<ConditionAttributes>> _conditionAttributes = new();

    public ModuleConditionHandler(
        IOptions<PipelineOptions> pipelineOptions,
        IOptions<DistributedOptions> distributedOptions,
        RoleDetector roleDetector,
        IPipelineContextProvider pipelineContextProvider,
        IModuleMetadataRegistry metadataRegistry)
    {
        _pipelineOptions = pipelineOptions;
        _distributedOptions = distributedOptions;
        _roleDetector = roleDetector;
        _pipelineContextProvider = pipelineContextProvider;
        _metadataRegistry = metadataRegistry;
    }

    public async Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnore(IModule module, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var evaluation = _conditionEvaluations.GetValue(module, static _ => new ConditionEvaluation());
        await evaluation.Gate.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (evaluation.HasResult)
            {
                return evaluation.Result;
            }

            var result = await EvaluateShouldIgnore(module, cancellationToken).ConfigureAwait(false);
            evaluation.Result = result;
            evaluation.HasResult = true;
            return result;
        }
        finally
        {
            evaluation.Gate.Release();
        }
    }

    public Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnoreByCategory(
        IModule module,
        CancellationToken cancellationToken = default)
        => ShouldIgnoreByCategory(module, _metadataRegistry, cancellationToken);

    public Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnoreByCategory(
        IModule module,
        IModuleMetadataRegistry metadataRegistry,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = EvaluateCategoryConditions(module, metadataRegistry);
        if (!result.ShouldIgnore
            && IsDistributedMaster()
            && OperatingSystemConditions.HasImpossibleCombination(GetConditionAttributes(module.GetType()).All))
        {
            result = (true, SkipDecision.Skip("Module requires mutually exclusive operating systems"));
        }

        return Task.FromResult(result);
    }

    public Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnoreForPlanning(
        IModule module,
        CancellationToken cancellationToken = default)
        => ShouldIgnoreForPlanning(module, _metadataRegistry, cancellationToken);

    public Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnoreForPlanning(
        IModule module,
        IModuleMetadataRegistry metadataRegistry,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return EvaluateShouldIgnore(
            module,
            cancellationToken,
            useFreshAttributes: true,
            metadataRegistry);
    }

    public async Task<PlanningConditionResult> ShouldIgnoreForGraphPlanning(
        IModule module,
        IModuleMetadataRegistry metadataRegistry,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var categoryResult = EvaluateCategoryConditions(module, metadataRegistry);
        if (categoryResult.ShouldIgnore)
        {
            return new PlanningConditionResult(
                true,
                categoryResult.SkipDecision,
                IsResolved: true);
        }

        return await EvaluatePlanningConditions(
                CreateConditionAttributes(module.GetType()),
                _pipelineContextProvider.GetModuleContext(),
                IsDistributedMaster(),
                cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> EvaluateShouldIgnore(
        IModule module,
        CancellationToken cancellationToken,
        bool useFreshAttributes = false,
        IModuleMetadataRegistry? metadataRegistry = null)
    {
        var categoryResult = EvaluateCategoryConditions(module, metadataRegistry ?? _metadataRegistry);
        if (categoryResult.ShouldIgnore)
        {
            return categoryResult;
        }

        var moduleType = module.GetType();
        var conditionResult = await IsRunnableCondition(
                moduleType,
                cancellationToken,
                useFreshAttributes)
            .ConfigureAwait(false);
        return conditionResult.IsRunnable
            ? (false, null)
            : (true, conditionResult.SkipDecision);
    }

    private (bool ShouldIgnore, SkipDecision? SkipDecision) EvaluateCategoryConditions(IModule module)
        => EvaluateCategoryConditions(module, _metadataRegistry);

    private (bool ShouldIgnore, SkipDecision? SkipDecision) EvaluateCategoryConditions(
        IModule module,
        IModuleMetadataRegistry metadataRegistry)
    {
        var moduleType = module.GetType();
        metadataRegistry.FinalizeMetadata(moduleType, module);
        var category = metadataRegistry.GetCategory(moduleType);

        if (IsIgnoreCategory(category))
        {
            return (true, SkipDecision.Skip("A category of this module has been ignored"));
        }

        if (!IsRunnableCategory(category))
        {
            return (true, SkipDecision.Skip("The module was not in a runnable category"));
        }

        return (false, null);
    }

    private bool IsRunnableCategory(string? category)
    {
        var runOnlyCategories = _pipelineOptions.Value.RunOnlyCategories?.ToArray();

        if (runOnlyCategories is not { Length: > 0 })
        {
            return true;
        }

        return category != null && runOnlyCategories.Contains(category, StringComparer.OrdinalIgnoreCase);
    }

    private bool IsIgnoreCategory(string? category)
    {
        var ignoreCategories = _pipelineOptions.Value.IgnoreCategories?.ToArray();

        if (ignoreCategories is not { Length: > 0 })
        {
            return false;
        }

        return category != null && ignoreCategories.Contains(category, StringComparer.OrdinalIgnoreCase);
    }

    private async Task<(bool IsRunnable, SkipDecision? SkipDecision)> IsRunnableCondition(
        Type moduleType,
        CancellationToken cancellationToken,
        bool useFreshAttributes)
    {
        var pipelineContext = _pipelineContextProvider.GetModuleContext();
        var attributes = useFreshAttributes
            ? CreateConditionAttributes(moduleType)
            : GetConditionAttributes(moduleType);
        return await EvaluateConditions(
            attributes,
            pipelineContext,
            IsDistributedMaster(),
            cancellationToken).ConfigureAwait(false);
    }

    private bool IsDistributedMaster()
    {
        var options = _distributedOptions.Value;
        return options.Enabled
               && options.TotalInstances > 1
               && _roleDetector.DetectRole() == DistributedRole.Master;
    }

    private ConditionAttributes GetConditionAttributes(Type moduleType)
    {
        return _conditionAttributes.GetOrAdd(
            moduleType,
            static type => new Lazy<ConditionAttributes>(
                () => CreateConditionAttributes(type),
                LazyThreadSafetyMode.ExecutionAndPublication)).Value;
    }

    private static ConditionAttributes CreateConditionAttributes(Type moduleType)
    {
        var attributes = moduleType.GetCustomAttributes(inherit: true);
        var conditionAttributes = attributes.OfType<IConditionAttribute>().ToArray();

        return new ConditionAttributes(
            conditionAttributes.Where(attribute => attribute.Logic == ConditionLogic.Skip).ToArray(),
            conditionAttributes.Where(attribute => attribute.Logic == ConditionLogic.All).ToArray(),
            conditionAttributes.Where(attribute => attribute.Logic == ConditionLogic.Any).ToArray());
    }

    private static bool IsPlanningConditionAttribute(IConditionAttribute attribute)
    {
        if (attribute is IPlanningConditionAttribute)
        {
            return true;
        }

        var conditionTypes = attribute.GetType().GetGenericArguments();
        return conditionTypes.Length > 0
               && conditionTypes.All(static type =>
                   typeof(IPlanningRunCondition).IsAssignableFrom(type));
    }

    private static async Task<PlanningConditionResult> EvaluatePlanningConditions(
        ConditionAttributes attributes,
        IPipelineContext pipelineContext,
        bool isDistributedMaster,
        CancellationToken cancellationToken)
    {
        var skipEvaluation = await EvaluateSkipPlanningConditions(
                attributes.Skip,
                pipelineContext,
                cancellationToken)
            .ConfigureAwait(false);
        if (skipEvaluation.Result is not null)
        {
            return skipEvaluation.Result;
        }

        var allEvaluation = await EvaluateAllPlanningConditions(
                attributes.All,
                pipelineContext,
                isDistributedMaster,
                cancellationToken)
            .ConfigureAwait(false);
        if (allEvaluation.Result is not null)
        {
            return allEvaluation.Result;
        }

        var anyEvaluation = await EvaluateAnyPlanningConditions(
                attributes.Any,
                pipelineContext,
                cancellationToken)
            .ConfigureAwait(false);
        return anyEvaluation.Result ?? new PlanningConditionResult(
            false,
            null,
            skipEvaluation.IsResolved
            && allEvaluation.IsResolved
            && anyEvaluation.IsResolved);
    }

    private static async Task<PlanningConditionEvaluation> EvaluateSkipPlanningConditions(
        IEnumerable<IConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        var isResolved = true;
        foreach (var attribute in attributes)
        {
            if (!IsPlanningConditionAttribute(attribute))
            {
                isResolved = false;
                continue;
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return new PlanningConditionEvaluation(
                    PlanningSkip($"SkipIf<{attribute.ConditionNames}> returned true"),
                    IsResolved: true);
            }
        }

        return new PlanningConditionEvaluation(null, isResolved);
    }

    private static async Task<PlanningConditionEvaluation> EvaluateAllPlanningConditions(
        IEnumerable<IConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        bool isDistributedMaster,
        CancellationToken cancellationToken)
    {
        var isResolved = true;
        var allConditions = attributes
            .Select(attribute => (
                Attribute: attribute,
                OperatingSystemTargets: OperatingSystemConditions.GetTargets(attribute)))
            .ToArray();
        var operatingSystemTargets = allConditions
            .SelectMany(condition => condition.OperatingSystemTargets)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var deferOperatingSystemConditions = isDistributedMaster && operatingSystemTargets.Length == 1;
        if (deferOperatingSystemConditions)
        {
            isResolved = false;
        }

        foreach (var (attribute, attributeOperatingSystemTargets) in allConditions)
        {
            if (deferOperatingSystemConditions && attributeOperatingSystemTargets.Count > 0)
            {
                continue;
            }

            if (!IsPlanningConditionAttribute(attribute))
            {
                isResolved = false;
                continue;
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (!await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return new PlanningConditionEvaluation(
                    PlanningSkip($"RunIfAll<{attribute.ConditionNames}> not satisfied"),
                    IsResolved: true);
            }
        }

        return new PlanningConditionEvaluation(null, isResolved);
    }

    private static async Task<PlanningConditionEvaluation> EvaluateAnyPlanningConditions(
        IReadOnlyCollection<IConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        var isResolved = true;
        var evaluatedGroups = new HashSet<Type>();
        foreach (var attribute in attributes)
        {
            PlanningConditionEvaluation evaluation;
            if (attribute is IGroupedConditionAttribute groupedAttribute)
            {
                if (!evaluatedGroups.Add(groupedAttribute.ConditionGroupType))
                {
                    continue;
                }

                evaluation = await EvaluateGroupedPlanningConditions(
                        attributes,
                        groupedAttribute.ConditionGroupType,
                        pipelineContext,
                        cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                evaluation = await EvaluateSingleAnyPlanningCondition(
                        attribute,
                        pipelineContext,
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            if (evaluation.Result is not null)
            {
                return evaluation;
            }

            isResolved &= evaluation.IsResolved;
        }

        return new PlanningConditionEvaluation(null, isResolved);
    }

    private static async Task<PlanningConditionEvaluation> EvaluateSingleAnyPlanningCondition(
        IConditionAttribute attribute,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        if (!IsPlanningConditionAttribute(attribute))
        {
            return new PlanningConditionEvaluation(null, IsResolved: false);
        }

        cancellationToken.ThrowIfCancellationRequested();
        var matches = await attribute.EvaluateAsync(pipelineContext, cancellationToken)
            .ConfigureAwait(false);
        return matches
            ? new PlanningConditionEvaluation(null, IsResolved: true)
            : new PlanningConditionEvaluation(
                PlanningSkip($"RunIfAny<{attribute.ConditionNames}> not satisfied"),
                IsResolved: true);
    }

    private static async Task<PlanningConditionEvaluation> EvaluateGroupedPlanningConditions(
        IEnumerable<IConditionAttribute> attributes,
        Type groupType,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        var alternatives = attributes
            .OfType<IGroupedConditionAttribute>()
            .Where(candidate => candidate.ConditionGroupType == groupType)
            .ToArray();
        var planningAlternatives = alternatives
            .Where(IsPlanningConditionAttribute)
            .ToArray();
        if (await AnyConditionMatches(
                planningAlternatives,
                pipelineContext,
                cancellationToken)
            .ConfigureAwait(false))
        {
            return new PlanningConditionEvaluation(null, IsResolved: true);
        }

        if (planningAlternatives.Length != alternatives.Length)
        {
            return new PlanningConditionEvaluation(null, IsResolved: false);
        }

        return new PlanningConditionEvaluation(
            PlanningSkip(
                $"No grouped run conditions were met: {string.Join(", ", alternatives.Select(x => x.ConditionNames))}"),
            IsResolved: true);
    }

    private static PlanningConditionResult PlanningSkip(string reason) =>
        new(true, SkipDecision.Skip(reason), IsResolved: true);

    private readonly record struct PlanningConditionEvaluation(
        PlanningConditionResult? Result,
        bool IsResolved);

    private static async Task<(bool IsRunnable, SkipDecision? SkipDecision)> EvaluateConditions(
        ConditionAttributes attributes,
        IPipelineContext pipelineContext,
        bool isDistributedMaster,
        CancellationToken cancellationToken)
    {
        var skipDecision = await EvaluateSkipConditions(
            attributes.Skip,
            pipelineContext,
            cancellationToken).ConfigureAwait(false);
        skipDecision ??= await EvaluateAllConditions(
            attributes.All,
            pipelineContext,
            isDistributedMaster,
            cancellationToken).ConfigureAwait(false);
        skipDecision ??= await EvaluateAnyConditions(
            attributes.Any,
            pipelineContext,
            cancellationToken).ConfigureAwait(false);

        return skipDecision is null ? (true, null) : (false, skipDecision);
    }

    private static async Task<SkipDecision?> EvaluateSkipConditions(
        IEnumerable<IConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        foreach (var attribute in attributes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return SkipDecision.Skip($"SkipIf<{attribute.ConditionNames}> returned true");
            }
        }

        return null;
    }

    private static async Task<SkipDecision?> EvaluateAllConditions(
        IEnumerable<IConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        bool isDistributedMaster,
        CancellationToken cancellationToken)
    {
        var allConditions = attributes
            .Select(attribute => (Attribute: attribute, OperatingSystemTargets: OperatingSystemConditions.GetTargets(attribute)))
            .ToArray();
        var operatingSystemTargets = allConditions
            .SelectMany(condition => condition.OperatingSystemTargets)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var deferOperatingSystemConditions = isDistributedMaster && operatingSystemTargets.Length == 1;

        foreach (var (attribute, attributeOperatingSystemTargets) in allConditions)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (deferOperatingSystemConditions && attributeOperatingSystemTargets.Count > 0)
            {
                continue;
            }

            if (!await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return SkipDecision.Skip($"RunIfAll<{attribute.ConditionNames}> not satisfied");
            }
        }

        return null;
    }

    private static async Task<SkipDecision?> EvaluateAnyConditions(
        IReadOnlyList<IConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        var evaluatedGroups = new HashSet<Type>();

        foreach (var attribute in attributes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (attribute is not IGroupedConditionAttribute groupedAttribute)
            {
                if (!await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
                {
                    return SkipDecision.Skip($"RunIfAny<{attribute.ConditionNames}> not satisfied");
                }

                continue;
            }

            if (!evaluatedGroups.Add(groupedAttribute.ConditionGroupType))
            {
                continue;
            }

            var alternatives = attributes
                .OfType<IGroupedConditionAttribute>()
                .Where(candidate => candidate.ConditionGroupType == groupedAttribute.ConditionGroupType)
                .ToArray();

            if (!await AnyConditionMatches(alternatives, pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return SkipDecision.Skip(
                    $"No grouped run conditions were met: {string.Join(", ", alternatives.Select(x => x.ConditionNames))}");
            }
        }

        return null;
    }

    private static async Task<bool> AnyConditionMatches(
        IEnumerable<IGroupedConditionAttribute> alternatives,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        foreach (var alternative in alternatives)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await alternative.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return true;
            }
        }

        return false;
    }

    private sealed class ConditionEvaluation
    {
        public SemaphoreSlim Gate { get; } = new(1, 1);

        public bool HasResult { get; set; }

        public (bool ShouldIgnore, SkipDecision? SkipDecision) Result { get; set; }
    }

    private sealed record ConditionAttributes(
        IConditionAttribute[] Skip,
        IConditionAttribute[] All,
        IConditionAttribute[] Any);
}
