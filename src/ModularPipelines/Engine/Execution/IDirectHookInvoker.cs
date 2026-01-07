using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Invokes direct module lifecycle hooks (virtual method overrides).
/// </summary>
internal interface IDirectHookInvoker
{
    /// <summary>
    /// Invokes the OnBeforeExecuteAsync hook on the module.
    /// </summary>
    /// <typeparam name="T">The module result type.</typeparam>
    /// <param name="module">The module to invoke the hook on.</param>
    /// <param name="context">The module context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    Task InvokeBeforeExecuteAsync<T>(
        Module<T> module,
        IModuleContext context,
        CancellationToken cancellationToken);

    /// <summary>
    /// Invokes the OnAfterExecuteAsync hook on the module.
    /// </summary>
    /// <typeparam name="T">The module result type.</typeparam>
    /// <param name="module">The module to invoke the hook on.</param>
    /// <param name="context">The module context.</param>
    /// <param name="result">The module result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A modified result, or null to keep the original.</returns>
    Task<ModuleResult<T>?> InvokeAfterExecuteAsync<T>(
        Module<T> module,
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken);

    /// <summary>
    /// Invokes the OnSkippedAsync hook on the module.
    /// </summary>
    /// <typeparam name="T">The module result type.</typeparam>
    /// <param name="module">The module to invoke the hook on.</param>
    /// <param name="context">The module context.</param>
    /// <param name="skipDecision">The skip decision with reason.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    Task InvokeSkippedAsync<T>(
        Module<T> module,
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken);

    /// <summary>
    /// Invokes the OnFailedAsync hook on the module.
    /// </summary>
    /// <typeparam name="T">The module result type.</typeparam>
    /// <param name="module">The module to invoke the hook on.</param>
    /// <param name="context">The module context.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    Task InvokeFailedAsync<T>(
        Module<T> module,
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken);
}
