using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Events;
using ModularPipelines.Logging;
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
        _pipelineEventHandlers = [.. pipelineEventHandlers.OrderBy(static handler => handler.Order)];
        _moduleEventHandlers = [.. moduleEventHandlers.OrderBy(static handler => handler.Order)];
        _eventHandlerInvoker = eventHandlerInvoker;
        _moduleContextProvider = moduleContextProvider;
        _metadataRegistry = metadataRegistry;
        _attributeEventService = attributeEventService;
    }

    public Task OnPipelineStartAsync(CancellationToken cancellationToken)
    {
        return _pipelineEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokePipelineStartHandlersAsync(
                _pipelineEventHandlers,
                GetPipelineContext(),
                cancellationToken);
    }

    public Task OnPipelineEndAsync(PipelineSummary pipelineSummary, CancellationToken cancellationToken)
    {
        return _pipelineEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokePipelineEndHandlersAsync(
                _pipelineEventHandlers,
                GetPipelineContext(),
                pipelineSummary,
                cancellationToken);
    }

    public Task OnModuleReadyAsync(
        ModuleState moduleState,
        IConsoleWriter consoleWriter,
        CancellationToken cancellationToken)
    {
        return _moduleEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokeReadyHandlersAsync(
                _moduleEventHandlers,
                CreateModuleHookContext(moduleState, moduleState.Result, consoleWriter),
                cancellationToken);
    }

    public Task OnModuleStartAsync(
        ModuleState moduleState,
        IConsoleWriter consoleWriter,
        CancellationToken cancellationToken)
    {
        return _moduleEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokeStartHandlersAsync(
                _moduleEventHandlers,
                CreateModuleHookContext(moduleState, moduleState.Result, consoleWriter),
                cancellationToken);
    }

    public Task OnModuleEndAsync(
        ModuleState moduleState,
        IModuleResult result,
        IConsoleWriter consoleWriter,
        CancellationToken cancellationToken)
    {
        return _moduleEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokeEndHandlersAsync(
                _moduleEventHandlers,
                CreateModuleHookContext(moduleState, result, consoleWriter),
                result,
                cancellationToken);
    }

    public Task OnModuleFailureAsync(
        ModuleState moduleState,
        Exception exception,
        IConsoleWriter consoleWriter,
        CancellationToken cancellationToken)
    {
        return _moduleEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokeFailureHandlersAsync(
                _moduleEventHandlers,
                CreateModuleHookContext(moduleState, moduleState.Result, consoleWriter),
                exception,
                cancellationToken);
    }

    public Task OnModuleSkippedAsync(
        ModuleState moduleState,
        IModuleResult result,
        SkipDecision reason,
        IConsoleWriter consoleWriter,
        CancellationToken cancellationToken)
    {
        return _moduleEventHandlers.Count == 0
            ? Task.CompletedTask
            : _eventHandlerInvoker.InvokeSkippedHandlersAsync(
                _moduleEventHandlers,
                CreateModuleHookContext(moduleState, result, consoleWriter),
                reason,
                cancellationToken);
    }

    private IPipelineContext GetPipelineContext()
    {
        return _moduleContextProvider.GetModuleContext();
    }

    private ModuleHookContext CreateModuleHookContext(
        ModuleState moduleState,
        IModuleResult? result,
        IConsoleWriter consoleWriter)
    {
        var moduleType = moduleState.ModuleType;
        var moduleAttributes = _attributeEventService.GetAttributes(moduleType);
        var startTime = moduleState.ExecutionStartTime ?? moduleState.QueuedTime ?? DateTimeOffset.UtcNow;

        return new ModuleHookContext(
            moduleState.Module,
            moduleAttributes,
            startTime,
            result,
            GetPipelineContext(),
            _metadataRegistry,
            consoleWriter);
    }
}
