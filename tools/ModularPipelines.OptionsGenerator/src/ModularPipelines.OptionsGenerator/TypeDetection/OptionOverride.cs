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
    /// Overrides whether the option value is secret.
    /// </summary>
    public bool? IsSecret { get; set; }

    /// <summary>
    /// Keys whose values are secret for a key-value option.
    /// </summary>
    public string[]? SecretValueKeys { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether collection values share one option occurrence.
    /// </summary>
    public bool GroupValues { get; set; }

    /// <summary>
    /// Optional reason for the override (for documentation).
    /// </summary>
    public string? Reason { get; set; }
}
