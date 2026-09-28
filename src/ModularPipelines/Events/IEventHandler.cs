namespace ModularPipelines.Events;

/// <summary>
/// Defines behavior shared by pipeline and module event handlers.
/// </summary>
public interface IEventHandler
{
    /// <summary>
    /// Gets whether a failure of this handler is only logged as a warning.
    /// </summary>
    /// <remarks>
    /// When <see langword="false"/> (the default), a failing Ready, Start, registration or pipeline
    /// handler fails the module or pipeline, and a failing End, Failure or Skipped handler is logged and
    /// recorded as a pipeline error without changing the module outcome it observed. A pipeline end
    /// handler never replaces an exception that already failed the pipeline.
    /// </remarks>
    bool ContinueOnError => false;

    /// <summary>
    /// Gets the invocation order within this handler's family. Handlers run in ascending order,
    /// so lower values run first. The default is 0.
    /// </summary>
    /// <remarks>
    /// Extension ordering (event handlers, requirements and validators) is ascending by <c>Order</c>.
    /// Module scheduling priority (<c>ModulePriority</c>) is a separate concept: higher priorities start first.
    /// </remarks>
    int Order => 0;
}
