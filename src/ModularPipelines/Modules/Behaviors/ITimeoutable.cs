namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to specify a custom timeout for module execution.
/// </summary>
/// <remarks>
/// <para>
/// <strong>Configuration Precedence:</strong>
/// Implementing this interface on a module takes precedence over the system default timeout.
/// </para>
/// <para>
/// <strong>Precedence order (highest to lowest):</strong>
/// </para>
/// <list type="number">
/// <item>Module-level: This interface implementation (highest priority)</item>
/// <item>System default: 30 minutes (lowest priority)</item>
/// </list>
/// <para>
/// When a module times out, it will throw a <see cref="Exceptions.ModuleTimeoutException"/>.
/// </para>
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
