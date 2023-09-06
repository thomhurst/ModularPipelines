using ModularPipelines.Interfaces;
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
        return Task.WhenAll(_globalHooks.Select(x => x.OnStartAsync(_moduleContextProvider.GetModuleContext())));
    }

    public Task OnEndAsync(IReadOnlyList<ModuleBase> modules)
    {
        return Task.WhenAll(_globalHooks.Select(x => x.OnEndAsync(_moduleContextProvider.GetModuleContext(), modules)));
    }

    public Task OnBeforeModuleStartAsync(ModuleBase module)
    {
        return Task.WhenAll(_moduleHooks.Select(x => x.OnBeforeModuleStartAsync(_moduleContextProvider.GetModuleContext(), module)));
    }

    public Task OnAfterModuleEndAsync(ModuleBase module)
    {
        return Task.WhenAll(_moduleHooks.Select(x => x.OnBeforeModuleEndAsync(_moduleContextProvider.GetModuleContext(), module)));
    }
}
