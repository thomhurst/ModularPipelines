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
        if (IsIgnoreCategory(module) || !IsRunnableCategory(module) || !await IsRunnableCondition(module))
        {
            module.SetSkipped();
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

    private async Task<bool> IsRunnableCondition(ModuleBase module)
    {
        var mandatoryRunConditionAttributes = module.GetType().GetCustomAttributes<MandatoryRunConditionAttribute>(true).ToList();
        var runConditionAttributes = module.GetType().GetCustomAttributes<RunConditionAttribute>(true).Except(mandatoryRunConditionAttributes).ToList();

        var mandatoryConditionResults = await mandatoryRunConditionAttributes.ToAsyncProcessorBuilder()
            .SelectAsync(async runConditionAttribute => await runConditionAttribute.Condition(module.Context))
            .ProcessInParallel();

        if (mandatoryConditionResults.Any(result => !result))
        {
            return false;
        }
        
        if (!runConditionAttributes.Any())
        {
            return true;
        }
        
        var conditionResults = await runConditionAttributes.ToAsyncProcessorBuilder()
            .SelectAsync(async runConditionAttribute => await runConditionAttribute.Condition(module.Context))
            .ProcessInParallel();

        return conditionResults.Any(result => result);
    }
}
