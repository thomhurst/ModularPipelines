namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a plugin declares compatibility with a different major version of ModularPipelines.
/// </summary>
public class PluginVersionMismatchException : PipelineException
{
    /// <summary>
    /// The name of the incompatible plugin assembly.
    /// </summary>
    public string PluginName { get; }

    /// <summary>
    /// The major version the plugin requires.
    /// </summary>
    public int RequiredMajorVersion { get; }

    /// <summary>
    /// The actual version of ModularPipelines installed.
    /// </summary>
    public Version? ActualCoreVersion { get; }

    /// <summary>
    /// Creates a new plugin version mismatch exception.
    /// </summary>
    /// <param name="pluginName">The name of the incompatible plugin assembly.</param>
    /// <param name="requiredMajorVersion">The major version the plugin requires.</param>
    /// <param name="actualCoreVersion">The actual version of ModularPipelines installed.</param>
    public PluginVersionMismatchException(
        string pluginName,
        int requiredMajorVersion,
        Version? actualCoreVersion)
        : base(BuildMessage(pluginName, requiredMajorVersion, actualCoreVersion))
    {
        PluginName = pluginName;
        RequiredMajorVersion = requiredMajorVersion;
        ActualCoreVersion = actualCoreVersion;
    }

    private static string BuildMessage(
        string pluginName,
        int requiredMajorVersion,
        Version? actualCoreVersion)
    {
        var actualVersionStr = actualCoreVersion?.ToString() ?? "unknown";
        var actualMajor = actualCoreVersion?.Major.ToString() ?? "unknown";

        return $"Plugin '{pluginName}' requires ModularPipelines {requiredMajorVersion}.x, " +
               $"but version {actualVersionStr} is installed. " +
               $"Update the plugin to a version compatible with ModularPipelines {actualMajor}.x, " +
               $"or update ModularPipelines to version {requiredMajorVersion}.x.";
    }
}
