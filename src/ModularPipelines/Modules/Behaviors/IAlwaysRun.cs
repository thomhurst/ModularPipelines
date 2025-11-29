namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Marker interface indicating that a module should run even if the pipeline has failed.
/// </summary>
/// <remarks>
/// Use this for cleanup, notification, or reporting modules that must execute regardless of pipeline status.
///
/// Behavior:
/// <list type="bullet">
/// <item>Module will not be cancelled when another module fails</item>
/// <item>Module will wait for its dependencies but ignore their failures</item>
/// <item>Exceptions from AlwaysRun modules are collected but don't prevent other AlwaysRun modules</item>
/// </list>
/// </remarks>
public interface IAlwaysRun
{
    // Marker interface - no members required
}
