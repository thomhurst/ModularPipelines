using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class PipelineSetupExecutor : IPipelineSetupExecutor
{
    private readonly IEnumerable<IPipelineGlobalHooks> _globalHooks;
    private readonly IEnumerable<IPipelineModuleHooks> _moduleHooks;
    private readonly IPipelineContextProvider _moduleContextProvider;

    public PipelineSetupExecutor(IEnumerable<IPipelineGlobalHooks> globalHooks,
        IEnumerable<IPipelineModuleHooks> moduleHooks,
        IPipelineContextProvider moduleContextProvider)
    {
        _globalHooks = globalHooks;
        _moduleHooks = moduleHooks;
        _moduleContextProvider = moduleContextProvider;
    }

    public Task OnStartAsync()
    {
        return Task.WhenAll(_globalHooks.Select(x => x.OnStartAsync(GetModuleContext())));
    }

    public Task OnEndAsync(PipelineSummary pipelineSummary)
    {
        return Task.WhenAll(_globalHooks.Select(x => x.OnEndAsync(GetModuleContext(), pipelineSummary)));
    }

    public Task OnBeforeModuleStartAsync(IModule module)
    {
        return Task.WhenAll(_moduleHooks.Select(x => x.OnBeforeModuleStartAsync(GetModuleContext(), module)));
    }

    public Task OnAfterModuleEndAsync(IModule module)
    {
        return Task.WhenAll(_moduleHooks.Select(x => x.OnAfterModuleEndAsync(GetModuleContext(), module)));
    }

    private IPipelineHookContext GetModuleContext()
    {
        return _moduleContextProvider.GetModuleContext();
    }
}
