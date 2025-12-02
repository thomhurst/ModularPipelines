namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to specify a custom timeout for module execution.
/// </summary>
/// <remarks>
/// If not implemented, modules use a default timeout of 30 minutes.
/// When a module times out, it will throw a <see cref="Exceptions.ModuleTimeoutException"/>.
/// </remarks>
public interface ITimeoutable
{
    /// <summary>
    /// Gets the maximum duration this module is allowed to execute.
    /// </summary>
    /// <remarks>
    /// Use <see cref="TimeSpan.Zero"/> to disable timeout (not recommended for production).
    /// </remarks>
    TimeSpan Timeout { get; }
}
