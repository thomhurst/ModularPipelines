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

    private async Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> EvaluateShouldIgnore(
        IModule module,
        CancellationToken cancellationToken)
    {
        var moduleType = module.GetType();
        _metadataRegistry.FinalizeMetadata(moduleType, module);
        var category = _metadataRegistry.GetCategory(moduleType);

        if (IsIgnoreCategory(category))
        {
            return (true, SkipDecision.Skip("A category of this module has been ignored"));
        }

        if (!IsRunnableCategory(category))
        {
            return (true, SkipDecision.Skip("The module was not in a runnable category"));
        }

        var conditionResult = await IsRunnableCondition(moduleType, cancellationToken).ConfigureAwait(false);
        return conditionResult.IsRunnable
            ? (false, null)
            : (true, conditionResult.SkipDecision);
    }

    private bool IsRunnableCategory(string? category)
    {
        var runOnlyCategories = _pipelineOptions.Value.RunOnlyCategories?.ToArray();

        if (runOnlyCategories?.Any() != true)
        {
            return true;
        }

        return category != null && runOnlyCategories.Contains(category);
    }

    private bool IsIgnoreCategory(string? category)
    {
        var ignoreCategories = _pipelineOptions.Value.IgnoreCategories?.ToArray();

        if (ignoreCategories?.Any() != true)
        {
            return false;
        }

        return category != null && ignoreCategories.Contains(category);
    }

    private async Task<(bool IsRunnable, SkipDecision? SkipDecision)> IsRunnableCondition(
        Type moduleType,
        CancellationToken cancellationToken)
    {
        var pipelineContext = _pipelineContextProvider.GetModuleContext();
        var attributes = GetConditionAttributes(moduleType);
        var newStyleResult = await EvaluateNewStyleConditions(attributes, pipelineContext, cancellationToken).ConfigureAwait(false);

        if (!newStyleResult.IsRunnable)
        {
            return newStyleResult;
        }

        return await EvaluateLegacyConditions(
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

#pragma warning disable CS0618 // Legacy condition attributes are cached and evaluated here to preserve compatibility.
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
        var newStyleAttributes = attributes.OfType<IConditionAttribute>().ToArray();
        var legacyAttributes = attributes.OfType<RunConditionAttribute>().ToArray();

        return new ConditionAttributes(
            newStyleAttributes.Where(attribute => attribute.Logic == ConditionLogic.Skip).ToArray(),
            newStyleAttributes.Where(attribute => attribute.Logic == ConditionLogic.All).ToArray(),
            newStyleAttributes.Where(attribute => attribute.Logic == ConditionLogic.Any).ToArray(),
            legacyAttributes.OfType<MandatoryRunConditionAttribute>().ToArray(),
            legacyAttributes.Where(attribute => attribute is not MandatoryRunConditionAttribute).ToArray());
    }

    private static async Task<(bool IsRunnable, SkipDecision? SkipDecision)> EvaluateNewStyleConditions(
        ConditionAttributes attributes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        foreach (var attribute in attributes.Skip)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return (false, SkipDecision.Skip($"SkipIf<{attribute.ConditionNames}> returned true"));
            }
        }

        foreach (var attribute in attributes.All)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return (false, SkipDecision.Skip($"RunIfAll<{attribute.ConditionNames}> not satisfied"));
            }
        }

        foreach (var attribute in attributes.Any)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return (false, SkipDecision.Skip($"RunIfAny<{attribute.ConditionNames}> not satisfied"));
            }
        }

        return (true, null);
    }

    private static async Task<(bool IsRunnable, SkipDecision? SkipDecision)> EvaluateLegacyConditions(
        ConditionAttributes attributes,
        IPipelineContext pipelineContext,
        bool isDistributedMaster,
        CancellationToken cancellationToken)
    {
        var allMandatoryConditions = attributes.Mandatory;

        // On a distributed master, OS-only conditions are normally deferred to the matching
        // worker (skipped here) so the master publishes the assignment with an OS capability
        // instead of skipping it locally. But if a module carries contradictory OS-only
        // conditions targeting more than one operating system (e.g. Windows-only AND
        // Linux-only, possibly via inheritance), no single worker can ever satisfy them.
        // In that case we must keep evaluating them here so the module is skipped everywhere,
        // otherwise the master would publish an assignment requiring multiple mutually
        // exclusive OS capabilities and wait forever for a result that never arrives.
        var targetsMultipleOperatingSystems = allMandatoryConditions
            .Select(OperatingSystemConditions.GetTarget)
            .Where(operatingSystem => operatingSystem is not null)
            .Distinct()
            .Count() > 1;

        var deferOperatingSystemConditionsToWorker = isDistributedMaster && !targetsMultipleOperatingSystems;

        var mandatoryConditions = allMandatoryConditions
            .Where(attribute => !deferOperatingSystemConditionsToWorker || OperatingSystemConditions.GetTarget(attribute) is null)
            .ToArray();
        var optionalConditions = attributes.Optional;

        foreach (var attribute in mandatoryConditions)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await attribute.Condition(pipelineContext).ConfigureAwait(false))
            {
                return (false, SkipDecision.Skip($"A condition to run this module has not been met - {attribute.GetType().Name}"));
            }
        }

        if (optionalConditions.Length == 0)
        {
            return (true, null);
        }

        foreach (var attribute in optionalConditions)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await attribute.Condition(pipelineContext).ConfigureAwait(false))
            {
                return (true, null);
            }
        }

        var names = optionalConditions.Select(attribute =>
            attribute.GetType().Name.Replace("Attribute", string.Empty, StringComparison.OrdinalIgnoreCase));
        return (false, SkipDecision.Skip($"No run conditions were met: {string.Join(", ", names)}"));
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
        MandatoryRunConditionAttribute[] Mandatory,
        RunConditionAttribute[] Optional);

#pragma warning restore CS0618
}
