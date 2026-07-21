using Microsoft.Extensions.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleConditionHandler : IModuleConditionHandler
{
    private readonly IOptions<PipelineOptions> _pipelineOptions;

    public ModuleConditionHandler(IOptions<PipelineOptions> pipelineOptions)
    {
        _pipelineOptions = pipelineOptions;
    }

    public Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnore(IModule module, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var category = module.Configuration.Category;

        if (IsIgnoreCategory(category))
        {
            return Task.FromResult<(bool, SkipDecision?)>((true, SkipDecision.Skip("A category of this module has been ignored")));
        }

        if (!IsRunnableCategory(category))
        {
            return Task.FromResult<(bool, SkipDecision?)>((true, SkipDecision.Skip("The module was not in a runnable category")));
        }

        return Task.FromResult<(bool, SkipDecision?)>((false, null));
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
}
