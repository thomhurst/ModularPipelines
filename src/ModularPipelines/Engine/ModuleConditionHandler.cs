using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Engine.Attributes;
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
    private readonly DistributedConditionRouting? _distributedConditionRouting;
    private readonly ConditionalWeakTable<IModule, ConditionEvaluation> _conditionEvaluations = new();
    private readonly ConcurrentDictionary<Type, Lazy<ConditionAttributes>> _conditionAttributes = new();

    public ModuleConditionHandler(
        IOptions<PipelineOptions> pipelineOptions,
        IOptions<DistributedOptions> distributedOptions,
        RoleDetector roleDetector,
        IPipelineContextProvider pipelineContextProvider,
        IModuleMetadataRegistry metadataRegistry,
        DistributedConditionRouting? distributedConditionRouting = null)
    {
        _pipelineOptions = pipelineOptions;
        _distributedOptions = distributedOptions;
        _roleDetector = roleDetector;
        _pipelineContextProvider = pipelineContextProvider;
        _metadataRegistry = metadataRegistry;
        _distributedConditionRouting = distributedConditionRouting;
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
            && OperatingSystemConditions.HasImpossibleCombination(module.GetType()))
        {
            result = (true, SkipDecision.Skip("Module requires mutually exclusive operating systems"));
        }

        return Task.FromResult(result);
    }

    public async Task PrepareDistributedRoutingAsync(
        IModule module,
        CancellationToken cancellationToken = default)
    {
        if (!IsDistributedMaster() || _distributedConditionRouting is null)
        {
            return;
        }

        await ShouldIgnore(module, cancellationToken).ConfigureAwait(false);
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
                CreatePlanningConditionAttributes(module.GetType()),
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
                module,
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
        IModule module,
        CancellationToken cancellationToken,
        bool useFreshAttributes)
    {
        var moduleType = module.GetType();
        var pipelineContext = _pipelineContextProvider.GetModuleContext();
        var attributes = useFreshAttributes
            ? CreateConditionAttributes(moduleType)
            : GetConditionAttributes(moduleType);
        return await EvaluateConditions(
            attributes,
            pipelineContext,
            IsDistributedMaster(),
            cancellationToken,
            conditionGroupType => _distributedConditionRouting?.IsLocallySatisfied(
                module,
                conditionGroupType) == true,
            conditionGroupType => _distributedConditionRouting?.MarkLocallySatisfied(
                module,
                conditionGroupType)).ConfigureAwait(false);
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

    private static ConditionAttributes CreatePlanningConditionAttributes(Type moduleType)
    {
        var conditionData = CustomAttributeMetadata.GetApplicable(
            moduleType,
            static type => typeof(IConditionAttribute).IsAssignableFrom(type));
        var planningAttributes = conditionData
            .Where(data => IsPlanningConditionAttribute(data.AttributeType))
            .Select(CustomAttributeMetadata.Create<IConditionAttribute>)
            .ToArray();
        var deferredTypes = conditionData
            .Select(static data => data.AttributeType)
            .Where(static type => !IsPlanningConditionAttribute(type))
            .ToArray();

        return new ConditionAttributes(
            planningAttributes.Where(attribute => attribute.Logic == ConditionLogic.Skip).ToArray(),
            planningAttributes.Where(attribute => attribute.Logic == ConditionLogic.All).ToArray(),
            planningAttributes.Where(attribute => attribute.Logic == ConditionLogic.Any).ToArray(),
            HasDeferredCondition(deferredTypes, ConditionLogic.Skip),
            HasDeferredCondition(deferredTypes, ConditionLogic.All),
            HasDeferredCondition(deferredTypes, ConditionLogic.Any),
            HasDeferredGroupedAnyCondition(deferredTypes));
    }

    private static bool HasDeferredCondition(
        IEnumerable<Type> attributeTypes,
        ConditionLogic logic) =>
        attributeTypes.Any(type => GetConditionLogic(type) is not { } conditionLogic
                                   || conditionLogic == logic);

    private static bool HasDeferredGroupedAnyCondition(IEnumerable<Type> attributeTypes) =>
        attributeTypes.Any(type =>
            typeof(IGroupedConditionAttribute).IsAssignableFrom(type)
            && (GetConditionLogic(type) is not { } logic || logic == ConditionLogic.Any));

    private static ConditionLogic? GetConditionLogic(Type attributeType)
    {
        if (typeof(SkipIfAttribute).IsAssignableFrom(attributeType))
        {
            return ConditionLogic.Skip;
        }

        if (typeof(RunIfAttribute).IsAssignableFrom(attributeType)
            || typeof(RunIfAllAttribute).IsAssignableFrom(attributeType))
        {
            return ConditionLogic.All;
        }

        return typeof(RunIfAnyAttribute).IsAssignableFrom(attributeType)
            ? ConditionLogic.Any
            : null;
    }

    private static bool IsPlanningConditionAttribute(IConditionAttribute attribute)
        => IsPlanningConditionAttribute(attribute.GetType());

    private static bool IsPlanningConditionAttribute(Type attributeType)
    {
        if (typeof(IPlanningConditionAttribute).IsAssignableFrom(attributeType))
        {
            return true;
        }

        var conditionTypes = attributeType.GetGenericArguments();
        return attributeType.IsGenericType
               && IsBuiltInGenericConditionAttribute(attributeType.GetGenericTypeDefinition())
               && conditionTypes.All(static type =>
                   typeof(IPlanningRunCondition).IsAssignableFrom(type));
    }

    private static bool IsBuiltInGenericConditionAttribute(Type type) =>
        type == typeof(RunIfAttribute<>)
        || type == typeof(RunIfAllAttribute<,>)
        || type == typeof(RunIfAllAttribute<,,>)
        || type == typeof(RunIfAllAttribute<,,,>)
        || type == typeof(RunIfAnyAttribute<,>)
        || type == typeof(RunIfAnyAttribute<,,>)
        || type == typeof(RunIfAnyAttribute<,,,>)
        || type == typeof(SkipIfAttribute<>)
        || type == typeof(SkipIfAttribute<,>)
        || type == typeof(SkipIfAttribute<,,>)
        || type == typeof(SkipIfAttribute<,,,>);

    private static async Task<PlanningConditionResult> EvaluatePlanningConditions(
        ConditionAttributes attributes,
        IPipelineContext pipelineContext,
        bool isDistributedMaster,
        CancellationToken cancellationToken)
    {
        var skipEvaluation = await EvaluateSkipPlanningConditions(
                attributes.Skip,
                pipelineContext,
                attributes.HasDeferredSkip,
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
                attributes.HasDeferredAll,
                cancellationToken)
            .ConfigureAwait(false);
        if (allEvaluation.Result is not null)
        {
            return allEvaluation.Result;
        }

        var anyEvaluation = await EvaluateAnyPlanningConditions(
                attributes.Any,
                pipelineContext,
                isDistributedMaster,
                attributes.HasDeferredAny,
                attributes.HasDeferredGroupedAny,
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
        bool hasDeferredConditions,
        CancellationToken cancellationToken)
    {
        var isResolved = !hasDeferredConditions;
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
        bool hasDeferredConditions,
        CancellationToken cancellationToken)
    {
        var isResolved = !hasDeferredConditions;
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
                    PlanningSkip($"{GetRequiredConditionName(attribute)}<{attribute.ConditionNames}> not satisfied"),
                    IsResolved: true);
            }
        }

        return new PlanningConditionEvaluation(null, isResolved);
    }

    private static async Task<PlanningConditionEvaluation> EvaluateAnyPlanningConditions(
        IReadOnlyCollection<IConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        bool isDistributedMaster,
        bool hasDeferredConditions,
        bool hasDeferredGroupedConditions,
        CancellationToken cancellationToken)
    {
        var isResolved = !hasDeferredConditions;
        foreach (var attribute in attributes.Where(static attribute =>
                     attribute is not IGroupedConditionAttribute))
        {
            if (ShouldDeferOperatingSystemCondition(attribute, isDistributedMaster))
            {
                isResolved = false;
                continue;
            }

            var evaluation = await EvaluateSingleAnyPlanningCondition(
                    attribute,
                    pipelineContext,
                    cancellationToken)
                .ConfigureAwait(false);
            if (evaluation.Result is not null)
            {
                return evaluation;
            }
        }

        var evaluatedGroups = new HashSet<Type>();
        foreach (var groupedAttribute in attributes.OfType<IGroupedConditionAttribute>())
        {
            if (!evaluatedGroups.Add(groupedAttribute.ConditionGroupType))
            {
                continue;
            }

            var evaluation = await EvaluateGroupedPlanningConditions(
                    attributes,
                    groupedAttribute.ConditionGroupType,
                    pipelineContext,
                    isDistributedMaster,
                    hasDeferredGroupedConditions,
                    cancellationToken)
                .ConfigureAwait(false);
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
        bool isDistributedMaster,
        bool hasDeferredGroupedConditions,
        CancellationToken cancellationToken)
    {
        var alternatives = attributes
            .OfType<IGroupedConditionAttribute>()
            .Where(candidate => candidate.ConditionGroupType == groupType)
            .ToArray();
        var planningAlternatives = alternatives
            .Where(attribute => !ShouldDeferOperatingSystemCondition(attribute, isDistributedMaster))
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

        if (hasDeferredGroupedConditions
            || planningAlternatives.Length != alternatives.Length)
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
        CancellationToken cancellationToken,
        Func<Type, bool>? isLocallySatisfiedConditionGroup = null,
        Action<Type>? locallySatisfiedConditionGroup = null)
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
            isDistributedMaster,
            cancellationToken,
            isLocallySatisfiedConditionGroup,
            locallySatisfiedConditionGroup).ConfigureAwait(false);

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
                return SkipDecision.Skip(
                    $"{GetRequiredConditionName(attribute)}<{attribute.ConditionNames}> not satisfied");
            }
        }

        return null;
    }

    private static string GetRequiredConditionName(IConditionAttribute attribute) =>
        attribute is RunIfAttribute ? "RunIf" : "RunIfAll";

    private static async Task<SkipDecision?> EvaluateAnyConditions(
        IReadOnlyList<IConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        bool isDistributedMaster,
        CancellationToken cancellationToken,
        Func<Type, bool>? isLocallySatisfiedConditionGroup,
        Action<Type>? locallySatisfiedConditionGroup)
    {
        var evaluatedGroups = new HashSet<Type>();

        foreach (var attribute in attributes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (attribute is not IGroupedConditionAttribute groupedAttribute)
            {
                var skipDecision = await EvaluateUngroupedAnyCondition(
                        attribute,
                        pipelineContext,
                        isDistributedMaster,
                        cancellationToken,
                        isLocallySatisfiedConditionGroup,
                        locallySatisfiedConditionGroup)
                    .ConfigureAwait(false);
                if (skipDecision is not null)
                {
                    return skipDecision;
                }

                continue;
            }

            if (!evaluatedGroups.Add(groupedAttribute.ConditionGroupType))
            {
                continue;
            }

            if (isLocallySatisfiedConditionGroup?.Invoke(groupedAttribute.ConditionGroupType) == true)
            {
                continue;
            }

            var alternatives = attributes
                .OfType<IGroupedConditionAttribute>()
                .Where(candidate => candidate.ConditionGroupType == groupedAttribute.ConditionGroupType)
                .ToArray();

            var localAlternatives = alternatives
                .Where(attribute =>
                    !ShouldDeferOperatingSystemCondition(attribute, isDistributedMaster))
                .ToArray();
            if (await AnyConditionMatches(
                    localAlternatives,
                    pipelineContext,
                    cancellationToken)
                .ConfigureAwait(false))
            {
                if (isDistributedMaster && localAlternatives.Length != alternatives.Length)
                {
                    locallySatisfiedConditionGroup?.Invoke(groupedAttribute.ConditionGroupType);
                }

                continue;
            }

            if (localAlternatives.Length != alternatives.Length)
            {
                continue;
            }

            return SkipDecision.Skip(
                $"No grouped run conditions were met: {string.Join(", ", alternatives.Select(x => x.ConditionNames))}");
        }

        return null;
    }

    private static async Task<SkipDecision?> EvaluateUngroupedAnyCondition(
        IConditionAttribute attribute,
        IPipelineContext pipelineContext,
        bool isDistributedMaster,
        CancellationToken cancellationToken,
        Func<Type, bool>? isLocallySatisfiedConditionGroup,
        Action<Type>? locallySatisfiedConditionGroup)
    {
        if (isLocallySatisfiedConditionGroup?.Invoke(attribute.GetType()) == true)
        {
            return null;
        }

        if (!ShouldDeferOperatingSystemCondition(attribute, isDistributedMaster))
        {
            return await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false)
                ? null
                : SkipDecision.Skip($"RunIfAny<{attribute.ConditionNames}> not satisfied");
        }

        if (await AnyConditionMatches(
                OperatingSystemConditions.GetLocalAlternatives(attribute),
                pipelineContext,
                cancellationToken)
            .ConfigureAwait(false))
        {
            locallySatisfiedConditionGroup?.Invoke(attribute.GetType());
        }

        return null;
    }

    private static bool ShouldDeferOperatingSystemCondition(
        IConditionAttribute attribute,
        bool isDistributedMaster) =>
        isDistributedMaster && OperatingSystemConditions.GetTargets(attribute).Count > 0;

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

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2067",
        Justification = "Condition types come from RunIfAny<T...> generic arguments with a new() constraint preserving public constructors.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2072",
        Justification = "Condition types come from RunIfAny<T...> generic arguments with a new() constraint preserving public constructors.")]
    private static async Task<bool> AnyConditionMatches(
        IEnumerable<Type> conditionTypes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        foreach (var conditionType in conditionTypes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var condition = (IRunCondition) Activator.CreateInstance(conditionType)!;
            if (await condition.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
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
        IConditionAttribute[] Any,
        bool HasDeferredSkip = false,
        bool HasDeferredAll = false,
        bool HasDeferredAny = false,
        bool HasDeferredGroupedAny = false);
}
