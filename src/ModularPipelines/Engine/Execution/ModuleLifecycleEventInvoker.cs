using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Logging;
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

        var readyTime = context.ReadyTime ?? context.StartTime;

        var hookContext = new ModuleHookContext(
            context.Module,
            context.ModuleAttributes,
            readyTime,
            result: null,
            context.PipelineContext,
            _metadataRegistry,
            GetConsoleWriter(context));

        await _eventHandlerInvoker.InvokeReadyHandlersAsync(handlers, hookContext).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeStartEventAsync(ModuleLifecycleContext context)
    {
        var handlers = _attributeEventService.GetStartHandlers(context.ModuleType);
        if (handlers.Count == 0)
        {
            return;
        }

        var hookContext = new ModuleHookContext(
            context.Module,
            context.ModuleAttributes,
            context.StartTime,
            result: null,
            context.PipelineContext,
            _metadataRegistry,
            GetConsoleWriter(context));

        await _eventHandlerInvoker.InvokeStartHandlersAsync(handlers, hookContext).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeEndEventAsync(ModuleLifecycleContext context, ModuleStatus status, IModuleResult result)
    {
        var handlers = _attributeEventService.GetEndHandlers(context.ModuleType);
        if (handlers.Count == 0)
        {
            return;
        }

        var hookContext = new ModuleHookContext(
            context.Module,
            context.ModuleAttributes,
            context.StartTime,
            result,
            context.PipelineContext,
            _metadataRegistry,
            GetConsoleWriter(context));

        await _eventHandlerInvoker.InvokeEndHandlersAsync(handlers, hookContext, result).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeFailedEventAsync(
        ModuleLifecycleContext context,
        IModuleResult result,
        Exception exception)
    {
        var handlers = _attributeEventService.GetFailureHandlers(context.ModuleType);
        if (handlers.Count == 0)
        {
            return;
        }

        var hookContext = new ModuleHookContext(
            context.Module,
            context.ModuleAttributes,
            context.StartTime,
            result,
            context.PipelineContext,
            _metadataRegistry,
            GetConsoleWriter(context));

        await _eventHandlerInvoker.InvokeFailureHandlersAsync(handlers, hookContext, exception).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeSkippedEventAsync(ModuleLifecycleContext context, ModuleStatus status, SkipDecision skipReason)
    {
        var handlers = _attributeEventService.GetSkippedHandlers(context.ModuleType);
        if (handlers.Count == 0)
        {
            return;
        }

        var hookContext = new ModuleHookContext(
            context.Module,
            context.ModuleAttributes,
            context.StartTime,
            result: null,
            context.PipelineContext,
            _metadataRegistry,
            GetConsoleWriter(context));

        await _eventHandlerInvoker.InvokeSkippedHandlersAsync(handlers, hookContext, skipReason).ConfigureAwait(false);
    }

    private static IConsoleWriter GetConsoleWriter(ModuleLifecycleContext context)
        => context.ScopedServiceProvider
               .GetRequiredService<IInternalModuleLoggerAccessor>()
               .GetLogger(context.ModuleType) as IConsoleWriter
           ?? context.PipelineContext.Console;
}
