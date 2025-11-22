using System.Reflection;
using Microsoft.Extensions.Options;
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

    public async Task<bool> ShouldIgnore(IModule module)
    {
        // Module<T> doesn't support SkipHandler/Context/RunConditions yet
        // TODO: Implement category/condition support for Module<T>
        return await Task.FromResult(false);
    }
}
