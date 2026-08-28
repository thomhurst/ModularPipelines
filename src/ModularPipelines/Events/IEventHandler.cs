namespace ModularPipelines.Events;

/// <summary>
/// Defines behavior shared by pipeline and module event handlers.
/// </summary>
public interface IEventHandler
{
    /// <summary>
    /// Gets whether execution continues when this handler throws.
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Gets the execution priority. Lower values run first.
    /// </summary>
    int Priority => 0;
}
