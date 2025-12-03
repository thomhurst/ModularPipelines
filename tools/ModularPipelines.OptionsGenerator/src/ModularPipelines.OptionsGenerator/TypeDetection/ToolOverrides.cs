namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// JSON structure for tool override files.
/// </summary>
public class ToolOverrides
{
    /// <summary>
    /// Global overrides that apply to all commands for this tool.
    /// Key is the option name (e.g., "--verbose").
    /// </summary>
    public Dictionary<string, OptionOverride> Global { get; set; } = new();

    /// <summary>
    /// Command-specific overrides.
    /// Key is the command path (e.g., "container.run").
    /// Value is a dictionary of option overrides.
    /// </summary>
    public Dictionary<string, Dictionary<string, OptionOverride>> Commands { get; set; } = new();
}
