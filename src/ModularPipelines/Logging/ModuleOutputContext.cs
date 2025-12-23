namespace ModularPipelines.Logging;

/// <summary>
/// Tracks module execution state for output formatting.
/// Used by <see cref="ModuleOutputWriter"/> to format section headers with duration and status.
/// </summary>
internal class ModuleOutputContext
{
    /// <summary>
    /// Gets the module name for section headers.
    /// </summary>
    public string ModuleName { get; }

    /// <summary>
    /// Gets the UTC time when the module started executing.
    /// </summary>
    public DateTime StartTimeUtc { get; }

    /// <summary>
    /// Gets or sets the exception if the module failed.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleOutputContext"/> class.
    /// </summary>
    /// <param name="moduleName">The module name.</param>
    public ModuleOutputContext(string moduleName)
    {
        ModuleName = moduleName;
        StartTimeUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the duration since the module started.
    /// </summary>
    public TimeSpan GetDuration() => DateTime.UtcNow - StartTimeUtc;

    /// <summary>
    /// Formats the section header with module name, status, and duration.
    /// </summary>
    /// <returns>Formatted section header string.</returns>
    public string FormatSectionHeader()
    {
        var duration = GetDuration();
        var durationStr = duration.TotalSeconds >= 60
            ? $"{duration.TotalMinutes:F1}m"
            : $"{duration.TotalSeconds:F1}s";

        if (Exception != null)
        {
            return $"{ModuleName} \u2717 ({durationStr}) - {Exception.GetType().Name}";
        }

        return $"{ModuleName} \u2713 ({durationStr})";
    }
}
