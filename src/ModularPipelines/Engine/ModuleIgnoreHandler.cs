using System.Reflection;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;

namespace ModularPipelines.Engine;

internal class ModuleIgnoreHandler : IModuleIgnoreHandler
{
    private readonly IOptions<PipelineOptions> _pipelineOptions;

    public ModuleIgnoreHandler(IOptions<PipelineOptions> pipelineOptions)
    {
        _pipelineOptions = pipelineOptions;
    }

    public async Task<bool> ShouldIgnore(ModuleBase module)
    {
        if (IsIgnoreCategory(module))
        {
            module.SetSkipped("A category of this module has been ignored");
            return true;
        }
        
        if (!IsRunnableCategory(module))
        {
            module.SetSkipped("The module was not in a runnable category");
            return true;
        }

        var (isRunnableCondition, runConditionAttribute) = await IsRunnableCondition(module);
        if (!isRunnableCondition)
        {
            module.SetSkipped($"A condition to run this module has not been met - {runConditionAttribute}");
            return true;
        }

        return false;
    }

    private bool IsRunnableCategory(ModuleBase module)
    {
        var runOnlyCategories = _pipelineOptions.Value.RunOnlyCategories?.ToArray();

        if (runOnlyCategories?.Any() != true)
        {
            return true;
        }

        var category = module.GetType().GetCustomAttribute<ModuleCategoryAttribute>();

        return category != null && runOnlyCategories.Contains(category.Category);
    }

    private bool IsIgnoreCategory(ModuleBase module)
    {
        var ignoreCategories = _pipelineOptions.Value.IgnoreCategories?.ToArray();

        if (ignoreCategories?.Any() != true)
        {
            return false;
        }

        var category = module.GetType().GetCustomAttribute<ModuleCategoryAttribute>();

        return category != null && ignoreCategories.Contains(category.Category);
    }

    private async Task<(bool, RunConditionAttribute?)> IsRunnableCondition(ModuleBase module)
    {
        var mandatoryRunConditionAttributes = module.GetType().GetCustomAttributes<MandatoryRunConditionAttribute>(true).ToList();
        var runConditionAttributes = module.GetType().GetCustomAttributes<RunConditionAttribute>(true).Except(mandatoryRunConditionAttributes).ToList();

        var mandatoryConditionResults = await mandatoryRunConditionAttributes.ToAsyncProcessorBuilder()
            .SelectAsync(async runConditionAttribute => new RunnableConditionMet(await runConditionAttribute.Condition(module.Context), runConditionAttribute))
            .ProcessInParallel();

        var mandatoryCondition = mandatoryConditionResults.FirstOrDefault(result => !result.ConditionMet);
        
        if (mandatoryCondition != null)
        {
            return (false, mandatoryCondition.RunConditionAttribute);
        }
        
        if (!runConditionAttributes.Any())
        {
            return (true, null);
        }
        
        var conditionResults = await runConditionAttributes.ToAsyncProcessorBuilder()
            .SelectAsync(async runConditionAttribute => new RunnableConditionMet(await runConditionAttribute.Condition(module.Context), runConditionAttribute))
            .ProcessInParallel();

        var runnableCondition = conditionResults.FirstOrDefault(result => result.ConditionMet);
        
        return (runnableCondition != null, null);
    }

    private record RunnableConditionMet(bool ConditionMet, RunConditionAttribute RunConditionAttribute);
}
