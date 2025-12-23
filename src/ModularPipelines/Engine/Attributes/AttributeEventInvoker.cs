using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Invokes attribute event receivers with configurable error handling.
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
                await receiver.OnRegistrationAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Registration receiver {Type} failed", receiver.GetType().Name);
                throw;
            }
        }
    }

    public async Task InvokeStartReceiversAsync(
        IEnumerable<IModuleStartEventReceiver> receivers,
        IModuleEventContext context)
    {
        foreach (var receiver in receivers)
        {
            try
            {
                await receiver.OnModuleStartAsync(context);
            }
            catch (Exception ex)
            {
                if (receiver.ContinueOnError)
                {
                    _logger.LogWarning(ex, "Start receiver {Type} failed, continuing", receiver.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public async Task InvokeEndReceiversAsync(
        IEnumerable<IModuleEndEventReceiver> receivers,
        IModuleEventContext context,
        ModuleResult result)
    {
        foreach (var receiver in receivers)
        {
            try
            {
                await receiver.OnModuleEndAsync(context, result);
            }
            catch (Exception ex)
            {
                if (receiver.ContinueOnError)
                {
                    _logger.LogWarning(ex, "End receiver {Type} failed, continuing", receiver.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public async Task InvokeFailureReceiversAsync(
        IEnumerable<IModuleFailureEventReceiver> receivers,
        IModuleEventContext context,
        Exception exception)
    {
        foreach (var receiver in receivers)
        {
            try
            {
                await receiver.OnModuleFailureAsync(context, exception);
            }
            catch (Exception ex)
            {
                if (receiver.ContinueOnError)
                {
                    _logger.LogWarning(ex, "Failure receiver {Type} failed, continuing", receiver.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public async Task InvokeSkippedReceiversAsync(
        IEnumerable<IModuleSkippedEventReceiver> receivers,
        IModuleEventContext context,
        SkipDecision reason)
    {
        foreach (var receiver in receivers)
        {
            try
            {
                await receiver.OnModuleSkippedAsync(context, reason);
            }
            catch (Exception ex)
            {
                if (receiver.ContinueOnError)
                {
                    _logger.LogWarning(ex, "Skipped receiver {Type} failed, continuing", receiver.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
