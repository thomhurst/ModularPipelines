using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;

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

    public Task OnBeforeModuleStartAsync(ModuleState moduleState)
    {
        return Task.WhenAll(_moduleHooks.Select(x => x.OnBeforeModuleStartAsync(GetModuleContext(), moduleState.Module)));
    }

    public Task OnAfterModuleEndAsync(ModuleState moduleState)
    {
        return Task.WhenAll(_moduleHooks.Select(x => x.OnAfterModuleEndAsync(GetModuleContext(), moduleState.Module)));
    }

    private IPipelineHookContext GetModuleContext()
    {
        return _moduleContextProvider.GetModuleContext();
    }
}