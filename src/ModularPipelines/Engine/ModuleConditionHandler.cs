using System.Reflection;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleConditionHandler : IModuleConditionHandler
{
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly IPipelineContextProvider _pipelineContextProvider;
    private readonly IModuleMetadataRegistry _metadataRegistry;

    public ModuleConditionHandler(
        IOptions<PipelineOptions> pipelineOptions,
        IPipelineContextProvider pipelineContextProvider,
        IModuleMetadataRegistry metadataRegistry)
    {
        _pipelineOptions = pipelineOptions;
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

        return await EvaluateLegacyConditions(moduleType, pipelineContext, cancellationToken).ConfigureAwait(false);
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
        CancellationToken cancellationToken)
    {
        var mandatoryConditions = moduleType.GetCustomAttributes<MandatoryRunConditionAttribute>(inherit: true).ToArray();
        var optionalConditions = moduleType
            .GetCustomAttributes<RunConditionAttribute>(inherit: true)
            .Except(mandatoryConditions)
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
}
