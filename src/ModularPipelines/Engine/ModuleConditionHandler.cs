using System.Reflection;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleConditionHandler : IModuleConditionHandler
{
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly IPipelineContextProvider _pipelineContextProvider;

    public ModuleConditionHandler(
        IOptions<PipelineOptions> pipelineOptions,
        IPipelineContextProvider pipelineContextProvider)
    {
        _pipelineOptions = pipelineOptions;
        _pipelineContextProvider = pipelineContextProvider;
    }

    public async Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnore(IModule module, CancellationToken cancellationToken = default)
    {
        var moduleType = module.GetType();

        if (IsIgnoreCategory(moduleType))
        {
            return (true, SkipDecision.Skip("A category of this module has been ignored"));
        }

        if (!IsRunnableCategory(moduleType))
        {
            return (true, SkipDecision.Skip("The module was not in a runnable category"));
        }

        var conditionResult = await IsRunnableCondition(moduleType, cancellationToken).ConfigureAwait(false);
        if (!conditionResult.IsRunnable)
        {
            return (true, conditionResult.SkipDecision);
        }

        return (false, null);
    }

    private bool IsRunnableCategory(Type moduleType)
    {
        var runOnlyCategories = _pipelineOptions.Value.RunOnlyCategories?.ToArray();

        if (runOnlyCategories?.Any() != true)
        {
            return true;
        }

        var category = moduleType.GetCustomAttribute<ModuleCategoryAttribute>();

        return category != null && runOnlyCategories.Contains(category.Category);
    }

    private bool IsIgnoreCategory(Type moduleType)
    {
        var ignoreCategories = _pipelineOptions.Value.IgnoreCategories?.ToArray();

        if (ignoreCategories?.Any() != true)
        {
            return false;
        }

        var category = moduleType.GetCustomAttribute<ModuleCategoryAttribute>();

        return category != null && ignoreCategories.Contains(category.Category);
    }

    private async Task<(bool IsRunnable, SkipDecision? SkipDecision)> IsRunnableCondition(Type moduleType, CancellationToken cancellationToken)
    {
        // Get a context for condition evaluation
        var pipelineContext = _pipelineContextProvider.GetModuleContext();

        // First, evaluate new-style IConditionAttribute attributes
        var newStyleResult = await EvaluateNewStyleConditions(moduleType, pipelineContext, cancellationToken).ConfigureAwait(false);
        if (!newStyleResult.IsRunnable)
        {
            return newStyleResult;
        }

        // Then, evaluate legacy RunConditionAttribute/MandatoryRunConditionAttribute for backwards compatibility
        var legacyResult = await EvaluateLegacyConditions(moduleType, pipelineContext, cancellationToken).ConfigureAwait(false);
        return legacyResult;
    }

    private static async Task<(bool IsRunnable, SkipDecision? SkipDecision)> EvaluateNewStyleConditions(
        Type moduleType,
        IPipelineHookContext pipelineContext,
        CancellationToken cancellationToken)
    {
        // Get all IConditionAttribute implementations
        var conditionAttributes = moduleType
            .GetCustomAttributes(true)
            .OfType<IConditionAttribute>()
            .ToArray();

        if (conditionAttributes.Length == 0)
        {
            return (true, null);
        }

        // 1. Evaluate SkipIf attributes first (fail-fast)
        var skipConditions = conditionAttributes.Where(a => a.Logic == ConditionLogic.Skip).ToArray();
        foreach (var attr in skipConditions)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await attr.EvaluateAsync(pipelineContext).ConfigureAwait(false))
            {
                return (false, SkipDecision.Skip($"SkipIf<{attr.ConditionNames}> returned true"));
            }
        }

        // 2. Evaluate RunIfAll attributes (all conditions in each must pass)
        var allConditions = conditionAttributes.Where(a => a.Logic == ConditionLogic.All).ToArray();
        foreach (var attr in allConditions)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await attr.EvaluateAsync(pipelineContext).ConfigureAwait(false))
            {
                return (false, SkipDecision.Skip($"RunIfAll<{attr.ConditionNames}> not satisfied"));
            }
        }

        // 3. Evaluate RunIfAny attributes (at least one condition must pass per attribute)
        var anyConditions = conditionAttributes.Where(a => a.Logic == ConditionLogic.Any).ToArray();
        foreach (var attr in anyConditions)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await attr.EvaluateAsync(pipelineContext).ConfigureAwait(false))
            {
                return (false, SkipDecision.Skip($"RunIfAny<{attr.ConditionNames}> not satisfied"));
            }
        }

        return (true, null);
    }

    private static async Task<(bool IsRunnable, SkipDecision? SkipDecision)> EvaluateLegacyConditions(
        Type moduleType,
        IPipelineHookContext pipelineContext,
        CancellationToken cancellationToken)
    {
        var mandatoryRunConditionAttributes = moduleType.GetCustomAttributes<MandatoryRunConditionAttribute>(true).ToArray();
        var runConditionAttributes = moduleType.GetCustomAttributes<RunConditionAttribute>(true).Except(mandatoryRunConditionAttributes).ToArray();

        // Evaluate mandatory conditions sequentially with short-circuit on first failure
        foreach (var attr in mandatoryRunConditionAttributes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var conditionMet = await attr.Condition(pipelineContext).ConfigureAwait(false);
            if (!conditionMet)
            {
                return (false, SkipDecision.Skip($"A condition to run this module has not been met - {attr.GetType().Name}"));
            }
        }

        if (runConditionAttributes.Length == 0)
        {
            return (true, null);
        }

        // Evaluate non-mandatory conditions sequentially with short-circuit on first success
        foreach (var attr in runConditionAttributes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var conditionMet = await attr.Condition(pipelineContext).ConfigureAwait(false);
            if (conditionMet)
            {
                return (true, null);
            }
        }

        return (false, SkipDecision.Skip($"No run conditions were met: {string.Join(", ", runConditionAttributes.Select(x => x.GetType().Name.Replace("Attribute", string.Empty, StringComparison.OrdinalIgnoreCase)))}"));
    }
}