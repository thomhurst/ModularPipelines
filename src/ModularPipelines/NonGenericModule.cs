using System.ComponentModel;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines;

/// <summary>
/// A pipeline module that performs work without returning a value.
/// </summary>
/// <remarks>
/// Use <see cref="Module"/> for cleanup, notification, publishing, and other operations
/// where dependent modules do not need a result value.
/// </remarks>
/// <example>
/// <code>
/// public class CleanupModule : Module
/// {
///     protected override Task ExecuteAsync(
///         IModuleContext context,
///         CancellationToken cancellationToken)
///     {
///         context.Files.GetFolder("./temp").Delete();
///         return Task.CompletedTask;
///     }
/// }
/// </code>
/// </example>
public abstract class Module : NonGenericModuleAdapter
{
    /// <summary>
    /// Executes the module's core logic.
    /// </summary>
    /// <param name="context">The module context providing access to pipeline services.</param>
    /// <param name="cancellationToken">A token that is cancelled if the pipeline fails or the module times out.</param>
    /// <returns>A task representing the operation.</returns>
    protected abstract new Task ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken);

    /// <inheritdoc />
    protected sealed override Task ExecuteWithoutResultAsync(
        IModuleContext context,
        CancellationToken cancellationToken) =>
        ExecuteAsync(context, cancellationToken);
}

/// <summary>
/// Infrastructure adapter for non-generic modules.
/// </summary>
/// <remarks>Inherit from <see cref="Module"/> instead.</remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class NonGenericModuleAdapter : Module<None>
{
    /// <summary>
    /// Executes the non-generic module implementation.
    /// </summary>
    protected abstract Task ExecuteWithoutResultAsync(
        IModuleContext context,
        CancellationToken cancellationToken);

    /// <inheritdoc />
    protected internal sealed override async Task<None> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        await ExecuteWithoutResultAsync(context, cancellationToken).ConfigureAwait(false);
        return None.Value;
    }
}
