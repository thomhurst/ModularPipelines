using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Invokes direct module lifecycle hooks (virtual method overrides) with proper error handling.
/// </summary>
internal class DirectHookInvoker : IDirectHookInvoker
{
    private readonly ILogger<DirectHookInvoker> _logger;

    public DirectHookInvoker(ILogger<DirectHookInvoker> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task InvokeBeforeExecuteAsync<T>(
        Module<T> module,
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        // OnBeforeExecuteAsync exceptions are propagated to prevent execution
        await module.InvokeOnBeforeExecuteAsync(context, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ModuleResult<T>?> InvokeAfterExecuteAsync<T>(
        Module<T> module,
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
    {
        try
        {
            return await module.InvokeOnAfterExecuteAsync(context, result, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log but don't propagate - preserve the original result
            _logger.LogError(ex, "OnAfterExecuteAsync hook failed for module {ModuleName}", module.GetType().Name);
            return null;
        }
    }

    /// <inheritdoc />
    public async Task InvokeSkippedAsync<T>(
        Module<T> module,
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
    {
        try
        {
            await module.InvokeOnSkippedAsync(context, skipDecision, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log but don't propagate - skip proceeds
            _logger.LogError(ex, "OnSkippedAsync hook failed for module {ModuleName}", module.GetType().Name);
        }
    }

    /// <inheritdoc />
    public async Task InvokeFailedAsync<T>(
        Module<T> module,
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        try
        {
            await module.InvokeOnFailedAsync(context, exception, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log but don't propagate - don't prevent OnAfterExecuteAsync from running
            _logger.LogError(ex, "OnFailedAsync hook failed for module {ModuleName}", module.GetType().Name);
        }
    }
}
