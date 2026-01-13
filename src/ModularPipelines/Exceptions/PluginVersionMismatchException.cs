namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a plugin declares compatibility with a different major version of ModularPipelines.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when a plugin (tool integration or extension) is loaded that
/// requires a different major version of the ModularPipelines core library than what is
/// currently installed. Major version mismatches can indicate breaking API changes.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>During plugin loading when version compatibility is checked</item>
/// <item>When a plugin assembly declares a different major version requirement</item>
/// </list>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="PluginName"/> - The name of the incompatible plugin assembly</item>
/// <item><see cref="RequiredMajorVersion"/> - The major version the plugin requires</item>
/// <item><see cref="ActualCoreVersion"/> - The actual version of ModularPipelines installed</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (PluginVersionMismatchException ex)
/// {
///     Console.WriteLine($"Plugin '{ex.PluginName}' requires ModularPipelines {ex.RequiredMajorVersion}.x");
///     Console.WriteLine($"Installed version: {ex.ActualCoreVersion}");
/// }
/// </code>
/// <para><b>Resolution:</b></para>
/// <list type="bullet">
/// <item>Update the plugin to a version compatible with your ModularPipelines version</item>
/// <item>Or update ModularPipelines to the version required by the plugin</item>
/// </list>
/// </remarks>
/// <seealso cref="PipelineException"/>
public class PluginVersionMismatchException : PipelineException
{
    /// <summary>
    /// Gets the name of the incompatible plugin assembly.
    /// </summary>
    public string PluginName { get; }

    /// <summary>
    /// Gets the major version the plugin requires.
    /// </summary>
    public int RequiredMajorVersion { get; }

    /// <summary>
    /// Gets the actual version of ModularPipelines installed.
    /// </summary>
    public Version? ActualCoreVersion { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginVersionMismatchException"/> class.
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
