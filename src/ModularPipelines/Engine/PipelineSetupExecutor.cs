using ModularPipelines.Context;
using ModularPipelines.Extensions;
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
        return Task.WhenAll(_globalHooks.Select(async x => x.OnStartAsync(await GetModuleContext())));
    }

    public Task OnEndAsync(PipelineSummary pipelineSummary)
    {
        return Task.WhenAll(_globalHooks.Select(async x => x.OnEndAsync(await GetModuleContext(), pipelineSummary)));
    }

    public Task OnBeforeModuleStartAsync(ModuleBase module)
    {
        return Task.WhenAll(_moduleHooks.Select(async x => x.OnBeforeModuleStartAsync(await GetModuleContext(), module)));
    }

    public Task OnAfterModuleEndAsync(ModuleBase module)
    {
        return Task.WhenAll(_moduleHooks.Select(async x => x.OnAfterModuleEndAsync(await GetModuleContext(), module)));
    }

    private async Task<IPipelineContext> GetModuleContext()
    {
        var pipelineContext = await _moduleContextProvider.GetModuleContext();
        return pipelineContext.ToNoModulePipelineContext();
    }
}
