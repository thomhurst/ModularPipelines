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
    private readonly IMetricsCollector _metricsCollector;
    private readonly IModuleMetadataRegistry _metadataRegistry;

    public ModuleLifecycleEventInvoker(
        IModuleAttributeEventService attributeEventService,
        IAttributeEventInvoker attributeEventInvoker,
        IMetricsCollector metricsCollector,
        IModuleMetadataRegistry metadataRegistry)
    {
        _attributeEventService = attributeEventService;
        _attributeEventInvoker = attributeEventInvoker;
        _metricsCollector = metricsCollector;
        _metadataRegistry = metadataRegistry;
    }

    /// <inheritdoc />
    public async Task InvokeReadyEventAsync(ModuleLifecycleContext context)
    {
        var receivers = _attributeEventService.GetReadyReceivers(context.ModuleType);
        if (receivers.Count == 0)
        {
            return;
        }

        var readyTime = context.ReadyTime ?? context.StartTime;
        var pipelineStartTime = _metricsCollector.GetPipelineStartTime() ?? readyTime;

        var eventContext = new ModuleReadyContext(
            context.Module,
            context.ModuleType,
            context.ModuleAttributes,
            readyTime,
            pipelineStartTime,
            context.PipelineContext,
            context.ScopedServiceProvider,
            _metadataRegistry,
            context.CancellationToken);

        await _attributeEventInvoker.InvokeReadyReceiversAsync(receivers, eventContext).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeStartEventAsync(ModuleLifecycleContext context)
    {
        var receivers = _attributeEventService.GetStartReceivers(context.ModuleType);
        if (receivers.Count == 0)
        {
            return;
        }

        var eventContext = new ModuleEventContext(
            context.Module,
            context.ModuleType,
            context.ModuleAttributes,
            context.StartTime,
            Enums.Status.Processing,
            context.PipelineContext,
            context.ScopedServiceProvider,
            _metadataRegistry,
            context.CancellationToken);

        await _attributeEventInvoker.InvokeStartReceiversAsync(receivers, eventContext).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeEndEventAsync(ModuleLifecycleContext context, Enums.Status status, IModuleResult result)
    {
        var receivers = _attributeEventService.GetEndReceivers(context.ModuleType);
        if (receivers.Count == 0)
        {
            return;
        }

        var eventContext = new ModuleEventContext(
            context.Module,
            context.ModuleType,
            context.ModuleAttributes,
            context.StartTime,
            status,
            context.PipelineContext,
            context.ScopedServiceProvider,
            _metadataRegistry,
            context.CancellationToken);

        await _attributeEventInvoker.InvokeEndReceiversAsync(receivers, eventContext, (ModuleResult)result).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeFailedEventAsync(ModuleLifecycleContext context, Exception exception)
    {
        var receivers = _attributeEventService.GetFailureReceivers(context.ModuleType);
        if (receivers.Count == 0)
        {
            return;
        }

        var eventContext = new ModuleEventContext(
            context.Module,
            context.ModuleType,
            context.ModuleAttributes,
            context.StartTime,
            Enums.Status.Failed,
            context.PipelineContext,
            context.ScopedServiceProvider,
            _metadataRegistry,
            context.CancellationToken);

        await _attributeEventInvoker.InvokeFailureReceiversAsync(receivers, eventContext, exception).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task InvokeSkippedEventAsync(ModuleLifecycleContext context, Enums.Status status, SkipDecision skipReason)
    {
        var receivers = _attributeEventService.GetSkippedReceivers(context.ModuleType);
        if (receivers.Count == 0)
        {
            return;
        }

        var eventContext = new ModuleEventContext(
            context.Module,
            context.ModuleType,
            context.ModuleAttributes,
            context.StartTime,
            status,
            context.PipelineContext,
            context.ScopedServiceProvider,
            _metadataRegistry,
            context.CancellationToken);

        await _attributeEventInvoker.InvokeSkippedReceiversAsync(receivers, eventContext, skipReason).ConfigureAwait(false);
    }
}
