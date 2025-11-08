namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to provide a custom timeout for a module.
/// If not implemented, a default timeout will be used (unless configured via attributes).
/// </summary>
public interface IModuleTimeout
{
    /// <summary>
    /// Gets the timeout duration for this module.
    /// Return TimeSpan.Zero to disable timeout.
    /// </summary>
    /// <returns>The timeout duration.</returns>
    TimeSpan GetTimeout();
}
