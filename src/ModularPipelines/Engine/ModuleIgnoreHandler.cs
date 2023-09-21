using System.Reflection;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleIgnoreHandler : IModuleIgnoreHandler
{
    private readonly IOptions<PipelineOptions> _pipelineOptions;

    public ModuleIgnoreHandler(IOptions<PipelineOptions> pipelineOptions)
    {
        _pipelineOptions = pipelineOptions;
    }

    public bool ShouldIgnore(ModuleBase module)
    {
        if (IsIgnoreCategory(module) || !IsRunnableCategory(module) || !IsRunnableCondition(module))
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

    private bool IsRunnableCondition(ModuleBase module)
    {
        var runOnOperatingSystemAttributes = module.GetType().GetCustomAttributes<RunConditionAttribute>(true).ToList();

        if (!runOnOperatingSystemAttributes.Any())
        {
            return true;
        }

        return runOnOperatingSystemAttributes.Any(attribute => attribute.Condition(module.Context));
    }
}
