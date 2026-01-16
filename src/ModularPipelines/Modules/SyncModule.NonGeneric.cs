using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

/// <summary>
/// A synchronous module that doesn't return a meaningful result.
/// </summary>
/// <remarks>
/// <para>
/// Use this class when your module performs a synchronous action (like simple file operations
/// or computations) but doesn't need to pass data to dependent modules.
/// </para>
/// <para>
/// For synchronous modules that return data, use <see cref="SyncModule{T}"/> instead.
/// For async modules, use <see cref="Module"/> or <see cref="Module{T}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LoggingModule : SyncModule
/// {
///     protected override void ExecuteModule(IModuleContext context, CancellationToken cancellationToken)
///     {
///         context.Logger.LogInformation("Pipeline executed at {Time}", DateTime.UtcNow);
///         // No return statement needed
///     }
/// }
/// </code>
/// </example>
public abstract class SyncModule : SyncModule<None>
{
    /// <summary>
    /// Executes the module's core logic synchronously.
    /// </summary>
    /// <param name="context">The module context providing access to pipeline services.</param>
    /// <param name="cancellationToken">A token that will be cancelled if the pipeline fails or the module times out.</param>
    /// <remarks>
    /// This method is called by the pipeline engine. Do not call it directly.
    /// Unlike <see cref="SyncModule{T}.Execute"/>, this method does not return a value.
    /// </remarks>
    protected abstract void ExecuteModule(IModuleContext context, CancellationToken cancellationToken);

    /// <inheritdoc />
    protected sealed override None Execute(IModuleContext context, CancellationToken cancellationToken)
    {
        ExecuteModule(context, cancellationToken);
        return None.Value;
    }
}
