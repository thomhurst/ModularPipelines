using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal class PipelineSetupExecutor : IPipelineSetupExecutor
{
    private readonly IEnumerable<IPipelineGlobalHooks> _globalHooks;
    private readonly IReadOnlyCollection<IPipelineModuleHooks> _moduleHooks;
    private readonly IPipelineContextProvider _moduleContextProvider;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IModuleAttributeEventService _attributeEventService;

    public PipelineSetupExecutor(IEnumerable<IPipelineGlobalHooks> globalHooks,
        IEnumerable<IPipelineModuleHooks> moduleHooks,
        IPipelineContextProvider moduleContextProvider,
        IModuleMetadataRegistry metadataRegistry,
        IModuleAttributeEventService attributeEventService)
    {
        _globalHooks = globalHooks;
        _moduleHooks = moduleHooks as IReadOnlyCollection<IPipelineModuleHooks> ?? moduleHooks.ToArray();
        _moduleContextProvider = moduleContextProvider;
        _metadataRegistry = metadataRegistry;
        _attributeEventService = attributeEventService;
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
        => InvokeModuleHooksAsync(
            moduleState,
            static (hook, context) => hook.OnModuleReadyAsync(context));

    public Task OnModuleStartAsync(ModuleState moduleState)
        => InvokeModuleHooksAsync(
            moduleState,
            static (hook, context) => hook.OnModuleStartAsync(context));

    public Task OnModuleEndAsync(ModuleState moduleState)
        => InvokeModuleHooksAsync(
            moduleState,
            static (hook, context) => hook.OnModuleEndAsync(context));

    public Task OnModuleFailureAsync(ModuleState moduleState)
        => InvokeModuleHooksAsync(
            moduleState,
            static (hook, context) => hook.OnModuleFailureAsync(context));

    public Task OnModuleSkippedAsync(ModuleState moduleState)
        => InvokeModuleHooksAsync(
            moduleState,
            static (hook, context) => hook.OnModuleSkippedAsync(context));

    private Task InvokeModuleHooksAsync(
        ModuleState moduleState,
        Func<IPipelineModuleHooks, IModuleHookContext, Task> invokeHook)
    {
        if (_moduleHooks.Count == 0)
        {
            return Task.CompletedTask;
        }

        var context = CreateModuleHookContext(moduleState);
        return Task.WhenAll(_moduleHooks.Select(hook => invokeHook(hook, context)));
    }

    private IPipelineContext GetPipelineContext()
    {
        return _moduleContextProvider.GetModuleContext();
    }

    private ModuleHookContext CreateModuleHookContext(ModuleState moduleState)
    {
        var moduleType = moduleState.ModuleType;
        var moduleAttributes = _attributeEventService.GetAttributes(moduleType);
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
