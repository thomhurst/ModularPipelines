using System.ComponentModel;
using ModularPipelines.Modules;

namespace ModularPipelines;

/// <summary>
/// A synchronous pipeline module that performs work without returning a value.
/// </summary>
/// <remarks>
/// Use <see cref="SyncModule"/> for synchronous cleanup, notification, publishing,
/// and other operations where dependent modules do not need a result value.
/// </remarks>
/// <example>
/// <code>
/// public class CleanupModule : SyncModule
/// {
///     protected override void Execute(
///         IModuleContext context,
///         CancellationToken cancellationToken)
///     {
///         context.Files.GetFolder("./temp").Delete();
///     }
/// }
/// </code>
/// </example>
public abstract class SyncModule : NonGenericSyncModuleAdapter
{
    /// <summary>
    /// Executes the module's core logic synchronously.
    /// </summary>
    /// <param name="context">The module context providing access to pipeline services.</param>
    /// <param name="cancellationToken">A token that is cancelled if the pipeline fails or the module times out.</param>
    protected abstract new void Execute(
        IModuleContext context,
        CancellationToken cancellationToken);

    /// <inheritdoc />
    protected sealed override void ExecuteWithoutResult(
        IModuleContext context,
        CancellationToken cancellationToken) =>
        Execute(context, cancellationToken);
}

/// <summary>
/// Infrastructure adapter for non-generic synchronous modules.
/// </summary>
/// <remarks>Inherit from <see cref="SyncModule"/> instead.</remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class NonGenericSyncModuleAdapter : SyncModule<None>
{
    /// <summary>
    /// Executes the non-generic synchronous module implementation.
    /// </summary>
    protected abstract void ExecuteWithoutResult(
        IModuleContext context,
        CancellationToken cancellationToken);

    /// <inheritdoc />
    protected sealed override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        ExecuteWithoutResult(context, cancellationToken);
        return None.Value;
    }
}
