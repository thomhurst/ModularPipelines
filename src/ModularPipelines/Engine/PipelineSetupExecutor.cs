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
        return Task.WhenAll(_globalHooks.Select(async x => x.OnStartAsync(await _moduleContextProvider.GetModuleContext())));
    }

    public Task OnEndAsync(PipelineSummary pipelineSummary)
    {
        return Task.WhenAll(_globalHooks.Select(async x => x.OnEndAsync(await _moduleContextProvider.GetModuleContext(), pipelineSummary)));
    }

    public Task OnBeforeModuleStartAsync(ModuleBase module)
    {
        return Task.WhenAll(_moduleHooks.Select(async x => x.OnBeforeModuleStartAsync(await _moduleContextProvider.GetModuleContext(), module)));
    }

    public Task OnAfterModuleEndAsync(ModuleBase module)
    {
        return Task.WhenAll(_moduleHooks.Select(async x => x.OnBeforeModuleEndAsync(await _moduleContextProvider.GetModuleContext(), module)));
    }
}
