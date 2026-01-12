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
    private readonly IAttributeEventInvoker _attributeEventInvoker;
    private readonly IModuleMetadataRegistry _metadataRegistry;

    public ModuleLifecycleEventInvoker(
        IModuleAttributeEventService attributeEventService,
        IAttributeEventInvoker attributeEventInvoker,
        IModuleMetadataRegistry metadataRegistry)
    {
        _attributeEventService = attributeEventService;
        _attributeEventInvoker = attributeEventInvoker;
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
            _metadataRegistry);

        await _attributeEventInvoker.InvokeReadyHandlersAsync(handlers, hookContext).ConfigureAwait(false);
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
            _metadataRegistry);

        await _attributeEventInvoker.InvokeStartHandlersAsync(handlers, hookContext).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeEndEventAsync(ModuleLifecycleContext context, Enums.Status status, IModuleResult result)
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
            _metadataRegistry);

        await _attributeEventInvoker.InvokeEndHandlersAsync(handlers, hookContext, result).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeFailedEventAsync(ModuleLifecycleContext context, Exception exception)
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
            result: null,
            context.PipelineContext,
            _metadataRegistry);

        await _attributeEventInvoker.InvokeFailureHandlersAsync(handlers, hookContext, exception).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeSkippedEventAsync(ModuleLifecycleContext context, Enums.Status status, SkipDecision skipReason)
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
            _metadataRegistry);

        await _attributeEventInvoker.InvokeSkippedHandlersAsync(handlers, hookContext, skipReason).ConfigureAwait(false);
    }
}
