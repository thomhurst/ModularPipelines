using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Invokes attribute event handlers with configurable error handling.
/// </summary>
internal class AttributeEventInvoker : IAttributeEventInvoker
{
    private readonly ILogger<AttributeEventInvoker> _logger;

    public AttributeEventInvoker(ILogger<AttributeEventInvoker> logger)
    {
        _logger = logger;
    }

    public async Task InvokeRegistrationReceiversAsync(
        IEnumerable<IModuleRegistrationEventReceiver> receivers,
        IModuleRegistrationContext context)
    {
        foreach (var receiver in receivers)
        {
            try
            {
                await receiver.OnRegistrationAsync(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Registration receiver {Type} failed", receiver.GetType().Name);
                throw;
            }
        }
    }

    public async Task InvokeReadyHandlersAsync(
        IEnumerable<IModuleReadyHandler> handlers,
        IModuleHookContext context)
    {
        foreach (var handler in handlers)
        {
            try
            {
                await handler.OnModuleReadyAsync(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (handler.ContinueOnError)
                {
                    _logger.LogWarning(ex, "Ready handler {Type} failed, continuing", handler.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public async Task InvokeStartHandlersAsync(
        IEnumerable<IModuleStartHandler> handlers,
        IModuleHookContext context)
    {
        foreach (var handler in handlers)
        {
            try
            {
                await handler.OnModuleStartAsync(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (handler.ContinueOnError)
                {
                    _logger.LogWarning(ex, "Start handler {Type} failed, continuing", handler.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public async Task InvokeEndHandlersAsync(
        IEnumerable<IModuleEndHandler> handlers,
        IModuleHookContext context,
        IModuleResult result)
    {
        foreach (var handler in handlers)
        {
            try
            {
                await handler.OnModuleEndAsync(context, result).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (handler.ContinueOnError)
                {
                    _logger.LogWarning(ex, "End handler {Type} failed, continuing", handler.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public async Task InvokeFailureHandlersAsync(
        IEnumerable<IModuleFailureHandler> handlers,
        IModuleHookContext context,
        Exception exception)
    {
        foreach (var handler in handlers)
        {
            try
            {
                await handler.OnModuleFailureAsync(context, exception).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (handler.ContinueOnError)
                {
                    _logger.LogWarning(ex, "Failure handler {Type} failed, continuing", handler.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public async Task InvokeSkippedHandlersAsync(
        IEnumerable<IModuleSkippedHandler> handlers,
        IModuleHookContext context,
        SkipDecision reason)
    {
        foreach (var handler in handlers)
        {
            try
            {
                await handler.OnModuleSkippedAsync(context, reason).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (handler.ContinueOnError)
                {
                    _logger.LogWarning(ex, "Skipped handler {Type} failed, continuing", handler.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
