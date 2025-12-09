namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Override definition for a single CLI option.
/// Used in JSON override files for manual type specifications.
/// </summary>
public class OptionOverride
{
    /// <summary>
    /// The correct type for this option.
    /// </summary>
    public CliOptionType Type { get; set; }

    /// <summary>
    /// Optional reason for the override (for documentation).
    /// </summary>
    public string? Reason { get; set; }
}
