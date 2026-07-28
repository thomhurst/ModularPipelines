using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal class PipelineSetupExecutor : IPipelineSetupExecutor
{
    private readonly IEnumerable<IPipelineGlobalHooks> _globalHooks;
    private readonly IReadOnlyCollection<IModuleEventReceiver> _moduleEventReceivers;
    private readonly IPipelineContextProvider _moduleContextProvider;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IModuleAttributeEventService _attributeEventService;

    public PipelineSetupExecutor(IEnumerable<IPipelineGlobalHooks> globalHooks,
        IEnumerable<IModuleEventReceiver> moduleEventReceivers,
        IPipelineContextProvider moduleContextProvider,
        IModuleMetadataRegistry metadataRegistry,
        IModuleAttributeEventService attributeEventService)
    {
        _globalHooks = globalHooks;
        _moduleEventReceivers = moduleEventReceivers as IReadOnlyCollection<IModuleEventReceiver>
            ?? moduleEventReceivers.ToArray();
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
        => InvokeModuleEventReceiversAsync(
            moduleState,
            static (receiver, context) => receiver.OnModuleReadyAsync(context));

    public Task OnModuleStartAsync(ModuleState moduleState)
        => InvokeModuleEventReceiversAsync(
            moduleState,
            static (receiver, context) => receiver.OnModuleStartAsync(context));

    public Task OnModuleEndAsync(ModuleState moduleState)
        => InvokeModuleEventReceiversAsync(
            moduleState,
            static (receiver, context) => receiver.OnModuleEndAsync(context));

    public Task OnModuleFailureAsync(ModuleState moduleState)
        => InvokeModuleEventReceiversAsync(
            moduleState,
            static (receiver, context) => receiver.OnModuleFailureAsync(context));

    public Task OnModuleSkippedAsync(ModuleState moduleState)
        => InvokeModuleEventReceiversAsync(
            moduleState,
            static (receiver, context) => receiver.OnModuleSkippedAsync(context));

    private Task InvokeModuleEventReceiversAsync(
        ModuleState moduleState,
        Func<IModuleEventReceiver, IModuleHookContext, Task> invokeReceiver)
    {
        if (_moduleEventReceivers.Count == 0)
        {
            return Task.CompletedTask;
        }

        var context = CreateModuleHookContext(moduleState);
        return Task.WhenAll(
            _moduleEventReceivers.Select(receiver => invokeReceiver(receiver, context)));
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
