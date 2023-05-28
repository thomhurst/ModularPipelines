using System.Reflection;
using Microsoft.Extensions.Options;
using Pipeline.NET.Attributes;
using Pipeline.NET.Modules;
using Pipeline.NET.Options;

namespace Pipeline.NET.Engine;

public class ModuleIgnoreHandler : IModuleIgnoreHandler
{
    private readonly IOptions<PipelineOptions> _pipelineOptions;

    public ModuleIgnoreHandler(IOptions<PipelineOptions> pipelineOptions)
    {
        _pipelineOptions = pipelineOptions;
    }
    
    public bool ShouldIgnore(IModule module)
    {
        return module.ShouldSkip || IsIgnoreCategory(module) || !IsRunnableCategory(module);
    }

    private bool IsRunnableCategory(IModule module)
    {
        var runOnlyCategories = _pipelineOptions.Value.RunOnlyCategories?.ToArray();
        
        if (runOnlyCategories?.Any() != true)
        {
            return true;
        }

        var category = module.GetType().GetCustomAttribute<ModuleCategoryAttribute>();

        return category != null && !runOnlyCategories.Contains(category.Category);
    }

    private bool IsIgnoreCategory(IModule module)
    {
        var ignoreCategories = _pipelineOptions.Value.IgnoreCategories?.ToArray();
        
        if (ignoreCategories?.Any() != true)
        {
            return false;
        }

        var category = module.GetType().GetCustomAttribute<ModuleCategoryAttribute>();

        return category != null && ignoreCategories.Contains(category.Category);
    }
}