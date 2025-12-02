using System.Reflection;
using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleConditionHandler : IModuleConditionHandler
{
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly IPipelineContextProvider _pipelineContextProvider;

    public ModuleConditionHandler(IOptions<PipelineOptions> pipelineOptions, IPipelineContextProvider pipelineContextProvider)
    {
        _pipelineOptions = pipelineOptions;
        _pipelineContextProvider = pipelineContextProvider;
    }

    public async Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnore(IModule module)
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

        var conditionResult = await IsRunnableCondition(moduleType);
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

    private async Task<(bool IsRunnable, SkipDecision? SkipDecision)> IsRunnableCondition(Type moduleType)
    {
        var mandatoryRunConditionAttributes = moduleType.GetCustomAttributes<MandatoryRunConditionAttribute>(true).ToList();
        var runConditionAttributes = moduleType.GetCustomAttributes<RunConditionAttribute>(true).Except(mandatoryRunConditionAttributes).ToList();

        // Get a context for condition evaluation
        var pipelineContext = _pipelineContextProvider.GetModuleContext();

        var mandatoryConditionResults = await mandatoryRunConditionAttributes.ToAsyncProcessorBuilder()
            .SelectAsync(async runConditionAttribute => new RunnableConditionMet(await runConditionAttribute.Condition(pipelineContext), runConditionAttribute))
            .ProcessInParallel();

        var mandatoryCondition = mandatoryConditionResults.FirstOrDefault(result => !result.ConditionMet);

        if (mandatoryCondition != null)
        {
            return (false, SkipDecision.Skip($"A condition to run this module has not been met - {mandatoryCondition.RunConditionAttribute.GetType().Name}"));
        }

        if (!runConditionAttributes.Any())
        {
            return (true, null);
        }

        var conditionResults = await runConditionAttributes.ToAsyncProcessorBuilder()
            .SelectAsync(async runConditionAttribute => new RunnableConditionMet(await runConditionAttribute.Condition(pipelineContext), runConditionAttribute))
            .ProcessInParallel();

        var runnableCondition = conditionResults.FirstOrDefault(result => result.ConditionMet);

        if (runnableCondition != null)
        {
            return (true, null);
        }

        return (false, SkipDecision.Skip($"No run conditions were met: {string.Join(", ", runConditionAttributes.Select(x => x.GetType().Name.Replace("Attribute", string.Empty, StringComparison.OrdinalIgnoreCase)))}"));
    }

    private record RunnableConditionMet(bool ConditionMet, RunConditionAttribute RunConditionAttribute);
}