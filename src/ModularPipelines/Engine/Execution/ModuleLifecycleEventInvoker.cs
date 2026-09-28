using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for invoking module lifecycle events.
/// </summary>
internal class ModuleLifecycleEventInvoker : IModuleLifecycleEventInvoker
{
    private readonly IModuleAttributeEventService _attributeEventService;
    private readonly IEventHandlerInvoker _eventHandlerInvoker;
    private readonly IModuleMetadataRegistry _metadataRegistry;

    public ModuleLifecycleEventInvoker(
        IModuleAttributeEventService attributeEventService,
        IEventHandlerInvoker eventHandlerInvoker,
        IModuleMetadataRegistry metadataRegistry)
    {
        _attributeEventService = attributeEventService;
        _eventHandlerInvoker = eventHandlerInvoker;
        _metadataRegistry = metadataRegistry;
    }

    /// <inheritdoc />
    public async Task InvokeReadyEventAsync(ModuleLifecycleContext context)
    {
        var handlers = _attributeEventService.GetReadyHandlers(context.ModuleType);
        if (handlers.Count == 0)
        {
            return;
        }

        var hookContext = CreateHookContext(context, context.ReadyTime ?? context.StartTime, result: null);
        await _eventHandlerInvoker
            .InvokeReadyHandlersAsync(handlers, hookContext, context.CancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeStartEventAsync(ModuleLifecycleContext context)
    {
        var handlers = _attributeEventService.GetStartHandlers(context.ModuleType);
        if (handlers.Count == 0)
        {
            return;
        }

        var hookContext = CreateHookContext(context, context.StartTime, result: null);
        await _eventHandlerInvoker
            .InvokeStartHandlersAsync(handlers, hookContext, context.CancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeEndEventAsync(
        ModuleLifecycleContext context,
        IModuleResult result,
        CancellationToken cancellationToken)
    {
        var handlers = _attributeEventService.GetEndHandlers(context.ModuleType);
        if (handlers.Count == 0)
        {
            return;
        }

        var hookContext = CreateHookContext(context, context.StartTime, result);
        await _eventHandlerInvoker
            .InvokeEndHandlersAsync(handlers, hookContext, result, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeFailedEventAsync(
        ModuleLifecycleContext context,
        IModuleResult result,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var handlers = _attributeEventService.GetFailureHandlers(context.ModuleType);
        if (handlers.Count == 0)
        {
            return;
        }

        var hookContext = CreateHookContext(context, context.StartTime, result);
        await _eventHandlerInvoker
            .InvokeFailureHandlersAsync(handlers, hookContext, exception, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeSkippedEventAsync(
        ModuleLifecycleContext context,
        IModuleResult result,
        SkipDecision skipReason,
        CancellationToken cancellationToken)
    {
        var handlers = _attributeEventService.GetSkippedHandlers(context.ModuleType);
        if (handlers.Count == 0)
        {
            return;
        }

        var hookContext = CreateHookContext(context, context.StartTime, result);
        await _eventHandlerInvoker
            .InvokeSkippedHandlersAsync(handlers, hookContext, skipReason, cancellationToken)
            .ConfigureAwait(false);
    }

    private ModuleHookContext CreateHookContext(
        ModuleLifecycleContext context,
        DateTimeOffset startTime,
        IModuleResult? result) =>
        new(
            context.Module,
            context.ModuleAttributes,
            startTime,
            result,
            context.PipelineContext,
            _metadataRegistry,
            context.ConsoleWriter);
}
