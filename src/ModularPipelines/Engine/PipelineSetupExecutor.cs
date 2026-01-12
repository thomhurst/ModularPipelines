using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal class PipelineSetupExecutor : IPipelineSetupExecutor
{
    private readonly IEnumerable<IPipelineGlobalHooks> _globalHooks;
    private readonly IEnumerable<IPipelineModuleHooks> _moduleHooks;
    private readonly IPipelineContextProvider _moduleContextProvider;
    private readonly IModuleMetadataRegistry _metadataRegistry;

    public PipelineSetupExecutor(IEnumerable<IPipelineGlobalHooks> globalHooks,
        IEnumerable<IPipelineModuleHooks> moduleHooks,
        IPipelineContextProvider moduleContextProvider,
        IModuleMetadataRegistry metadataRegistry)
    {
        _globalHooks = globalHooks;
        _moduleHooks = moduleHooks;
        _moduleContextProvider = moduleContextProvider;
        _metadataRegistry = metadataRegistry;
    }

    public Task OnPipelineStartAsync()
    {
        return Task.WhenAll(_globalHooks.Select(x => x.OnPipelineStartAsync(GetPipelineContext())));
    }

    public Task OnPipelineEndAsync(PipelineSummary pipelineSummary)
    {
        return Task.WhenAll(_globalHooks.Select(x => x.OnPipelineEndAsync(GetPipelineContext(), pipelineSummary)));
    }

    public Task OnModuleReadyAsync(ModuleState moduleState)
    {
        var context = CreateModuleHookContext(moduleState);
        return Task.WhenAll(_moduleHooks.Select(x => x.OnModuleReadyAsync(context)));
    }

    public Task OnModuleStartAsync(ModuleState moduleState)
    {
        var context = CreateModuleHookContext(moduleState);
        return Task.WhenAll(_moduleHooks.Select(x => x.OnModuleStartAsync(context)));
    }

    public Task OnModuleEndAsync(ModuleState moduleState)
    {
        var context = CreateModuleHookContext(moduleState);
        return Task.WhenAll(_moduleHooks.Select(x => x.OnModuleEndAsync(context)));
    }

    public Task OnModuleFailureAsync(ModuleState moduleState)
    {
        var context = CreateModuleHookContext(moduleState);
        return Task.WhenAll(_moduleHooks.Select(x => x.OnModuleFailureAsync(context)));
    }

    public Task OnModuleSkippedAsync(ModuleState moduleState)
    {
        var context = CreateModuleHookContext(moduleState);
        return Task.WhenAll(_moduleHooks.Select(x => x.OnModuleSkippedAsync(context)));
    }

    private IPipelineHookContext GetPipelineContext()
    {
        return _moduleContextProvider.GetModuleContext();
    }

    private ModuleHookContext CreateModuleHookContext(ModuleState moduleState)
    {
        var moduleType = moduleState.ModuleType;
        var moduleAttributes = Attribute.GetCustomAttributes(moduleType).ToList().AsReadOnly();
        var startTime = moduleState.ExecutionStartTime ?? moduleState.QueuedTime ?? DateTimeOffset.UtcNow;

        return new ModuleHookContext(
            moduleState.Module,
            moduleAttributes,
            startTime,
            moduleState.Result,
            GetPipelineContext(),
            _metadataRegistry);
    }
}
