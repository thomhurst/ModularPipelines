using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Events;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Invokes event handlers with configurable error handling.
/// </summary>
internal class EventHandlerInvoker : IEventHandlerInvoker
{
    private readonly ILogger<EventHandlerInvoker> _logger;

    public EventHandlerInvoker(ILogger<EventHandlerInvoker> logger)
    {
        _logger = logger;
    }

    public Task InvokePipelineStartHandlersAsync(
        IEnumerable<IPipelineEventHandler> handlers,
        IPipelineContext context,
        CancellationToken cancellationToken) =>
        InvokeHandlersAsync(handlers, handler => handler.OnPipelineStartAsync(context, cancellationToken), "Pipeline start");

    public Task InvokePipelineEndHandlersAsync(
        IEnumerable<IPipelineEventHandler> handlers,
        IPipelineContext context,
        PipelineSummary summary,
        CancellationToken cancellationToken) =>
        InvokeHandlersAsync(handlers, handler => handler.OnPipelineEndAsync(context, summary, cancellationToken), "Pipeline end");

    public Task InvokeRegistrationHandlersAsync(
        IEnumerable<IModuleRegistrationHandler> handlers,
        IModuleRegistrationContext context,
        CancellationToken cancellationToken) =>
        InvokeHandlersAsync(handlers, handler => handler.OnRegistrationAsync(context, cancellationToken), "Registration");

    public Task InvokeReadyHandlersAsync(
        IEnumerable<IModuleReadyHandler> handlers,
        IModuleHookContext context,
        CancellationToken cancellationToken) =>
        InvokeHandlersAsync(handlers, handler => handler.OnModuleReadyAsync(context, cancellationToken), "Ready");

    public Task InvokeStartHandlersAsync(
        IEnumerable<IModuleStartHandler> handlers,
        IModuleHookContext context,
        CancellationToken cancellationToken) =>
        InvokeHandlersAsync(handlers, handler => handler.OnModuleStartAsync(context, cancellationToken), "Start");

    public Task InvokeEndHandlersAsync(
        IEnumerable<IModuleEndHandler> handlers,
        IModuleHookContext context,
        IModuleResult result,
        CancellationToken cancellationToken) =>
        InvokeHandlersAsync(handlers, handler => handler.OnModuleEndAsync(context, result, cancellationToken), "End");

    public Task InvokeFailureHandlersAsync(
        IEnumerable<IModuleFailureHandler> handlers,
        IModuleHookContext context,
        Exception exception,
        CancellationToken cancellationToken) =>
        InvokeHandlersAsync(handlers, handler => handler.OnModuleFailureAsync(context, exception, cancellationToken), "Failure");

    public Task InvokeSkippedHandlersAsync(
        IEnumerable<IModuleSkippedHandler> handlers,
        IModuleHookContext context,
        SkipDecision reason,
        CancellationToken cancellationToken) =>
        InvokeHandlersAsync(handlers, handler => handler.OnModuleSkippedAsync(context, reason, cancellationToken), "Skipped");

    private async Task InvokeHandlersAsync<THandler>(
        IEnumerable<THandler> handlers,
        Func<THandler, Task> invoke,
        string eventName)
        where THandler : IEventHandler
    {
        List<Exception>? failures = null;

        foreach (var handler in handlers)
        {
            try
            {
                await invoke(handler).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (handler.ContinueOnError)
                {
                    _logger.LogWarning(
                        ex,
                        "{EventName} handler {Type} failed, continuing",
                        eventName,
                        handler.GetType().Name);
                }
                else
                {
                    _logger.LogError(
                        ex,
                        "{EventName} handler {Type} failed",
                        eventName,
                        handler.GetType().Name);
                    (failures ??= []).Add(ex);
                }
            }
        }

        if (failures is [var failure])
        {
            ExceptionDispatchInfo.Capture(failure).Throw();
        }

        if (failures is { Count: > 1 })
        {
            throw new AggregateException($"Multiple {eventName} handlers failed.", failures);
        }
    }
}
