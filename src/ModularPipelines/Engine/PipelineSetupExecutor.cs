using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Events;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal class PipelineSetupExecutor : IPipelineSetupExecutor
{
    private readonly IReadOnlyList<IPipelineEventHandler> _pipelineEventHandlers;
    private readonly IReadOnlyList<IModuleEventHandler> _moduleEventHandlers;
    private readonly IEventHandlerInvoker _eventHandlerInvoker;
    private readonly IPipelineContextProvider _moduleContextProvider;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IModuleAttributeEventService _attributeEventService;

    public PipelineSetupExecutor(
        IEnumerable<IPipelineEventHandler> pipelineEventHandlers,
        IEnumerable<IModuleEventHandler> moduleEventHandlers,
        IEventHandlerInvoker eventHandlerInvoker,
        IPipelineContextProvider moduleContextProvider,
        IModuleMetadataRegistry metadataRegistry,
        IModuleAttributeEventService attributeEventService)
    {
        _pipelineEventHandlers = [.. pipelineEventHandlers.OrderBy(static handler => handler.Priority)];
        _moduleEventHandlers = [.. moduleEventHandlers.OrderBy(static handler => handler.Priority)];
        _eventHandlerInvoker = eventHandlerInvoker;
        _moduleContextProvider = moduleContextProvider;
        _metadataRegistry = metadataRegistry;
        _attributeEventService = attributeEventService;
    }

    public Task OnPipelineStartAsync()
    {
        return _pipelineEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokePipelineStartHandlersAsync(
                _pipelineEventHandlers,
                GetPipelineContext());
    }

    public Task OnPipelineEndAsync(PipelineSummary pipelineSummary)
    {
        return _pipelineEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokePipelineEndHandlersAsync(
                _pipelineEventHandlers,
                GetPipelineContext(),
                pipelineSummary);
    }

    public Task OnModuleReadyAsync(ModuleState moduleState)
    {
        return _moduleEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokeReadyHandlersAsync(
                _moduleEventHandlers,
                CreateModuleHookContext(moduleState));
    }

    public Task OnModuleStartAsync(ModuleState moduleState)
    {
        return _moduleEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokeStartHandlersAsync(
                _moduleEventHandlers,
                CreateModuleHookContext(moduleState));
    }

    public Task OnModuleEndAsync(ModuleState moduleState, IModuleResult result)
    {
        return _moduleEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokeEndHandlersAsync(
                _moduleEventHandlers,
                CreateModuleHookContext(moduleState),
                result);
    }

    public Task OnModuleFailureAsync(ModuleState moduleState, Exception exception)
    {
        return _moduleEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokeFailureHandlersAsync(
                _moduleEventHandlers,
                CreateModuleHookContext(moduleState),
                exception);
    }

    public Task OnModuleSkippedAsync(ModuleState moduleState, SkipDecision reason)
    {
        return _moduleEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokeSkippedHandlersAsync(
                _moduleEventHandlers,
                CreateModuleHookContext(moduleState),
                reason);
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
