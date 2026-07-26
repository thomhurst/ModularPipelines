using System.Reflection;
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
        var newStyleResult = await EvaluateNewStyleConditions(moduleType, pipelineContext, cancellationToken).ConfigureAwait(false);

        if (!newStyleResult.IsRunnable)
        {
            return newStyleResult;
        }

        return await EvaluateLegacyConditions(
            moduleType,
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

    private static async Task<(bool IsRunnable, SkipDecision? SkipDecision)> EvaluateNewStyleConditions(
        Type moduleType,
        IPipelineHookContext pipelineContext,
        CancellationToken cancellationToken)
    {
        var conditionAttributes = moduleType
            .GetCustomAttributes(inherit: true)
            .OfType<IConditionAttribute>()
            .ToArray();

        foreach (var attribute in conditionAttributes.Where(attribute => attribute.Logic == ConditionLogic.Skip))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return (false, SkipDecision.Skip($"SkipIf<{attribute.ConditionNames}> returned true"));
            }
        }

        foreach (var attribute in conditionAttributes.Where(attribute => attribute.Logic == ConditionLogic.All))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return (false, SkipDecision.Skip($"RunIfAll<{attribute.ConditionNames}> not satisfied"));
            }
        }

        foreach (var attribute in conditionAttributes.Where(attribute => attribute.Logic == ConditionLogic.Any))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return (false, SkipDecision.Skip($"RunIfAny<{attribute.ConditionNames}> not satisfied"));
            }
        }

        return (true, null);
    }

#pragma warning disable CS0618 // Legacy conditions are evaluated here to preserve compatibility.
    private static async Task<(bool IsRunnable, SkipDecision? SkipDecision)> EvaluateLegacyConditions(
        Type moduleType,
        IPipelineHookContext pipelineContext,
        bool isDistributedMaster,
        CancellationToken cancellationToken)
    {
        var allMandatoryConditions = moduleType
            .GetCustomAttributes<MandatoryRunConditionAttribute>(inherit: true)
            .ToArray();

        // On a distributed master, OS-only conditions are normally deferred to the matching
        // worker (skipped here) so the master publishes the assignment with an OS capability
        // instead of skipping it locally. But if a module carries contradictory OS-only
        // conditions targeting more than one operating system (e.g. Windows-only AND
        // Linux-only, possibly via inheritance), no single worker can ever satisfy them.
        // In that case we must keep evaluating them here so the module is skipped everywhere,
        // otherwise the master would publish an assignment requiring multiple mutually
        // exclusive OS capabilities and wait forever for a result that never arrives.
        var targetsMultipleOperatingSystems = allMandatoryConditions
            .Select(GetOperatingSystemConditionTarget)
            .Where(operatingSystem => operatingSystem is not null)
            .Distinct()
            .Count() > 1;

        var deferOperatingSystemConditionsToWorker = isDistributedMaster && !targetsMultipleOperatingSystems;

        var mandatoryConditions = allMandatoryConditions
            .Where(attribute => !deferOperatingSystemConditionsToWorker || !IsOperatingSystemCondition(attribute))
            .ToArray();
        var optionalConditions = moduleType
            .GetCustomAttributes<RunConditionAttribute>(inherit: true)
            .Except(allMandatoryConditions)
            .ToArray();

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
#pragma warning restore CS0618

    private static bool IsOperatingSystemCondition(MandatoryRunConditionAttribute attribute)
    {
        return GetOperatingSystemConditionTarget(attribute) is not null;
    }

    /// <summary>
    /// Returns a stable identifier for the operating system an OS-only mandatory condition
    /// targets, or <see langword="null"/> if the attribute is not an OS-only condition.
    /// Pattern matching means subclasses of the OS-only attributes are classified by their
    /// base operating system, so contradictory combinations are detected even via inheritance.
    /// </summary>
    private static string? GetOperatingSystemConditionTarget(MandatoryRunConditionAttribute attribute) => attribute switch
    {
        RunOnWindowsOnlyAttribute => "windows",
        RunOnLinuxOnlyAttribute => "linux",
        RunOnMacOSOnlyAttribute => "macos",
        _ => null,
    };
}
