namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Result from an option type detector.
/// </summary>
public class OptionTypeDetectionResult
{
    /// <summary>
    /// The detected type for the option.
    /// </summary>
    public required CliOptionType Type { get; init; }

    /// <summary>
    /// Confidence level (0-100) in the detection.
    /// Higher values indicate more reliable detection.
    /// </summary>
    public required int Confidence { get; init; }

    /// <summary>
    /// Source of the detection (e.g., "HelpText", "ManualOverride", "WebScraping").
    /// </summary>
    public required string Source { get; init; }

    /// <summary>
    /// Optional additional information about the detection.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Creates a result indicating the detector could not determine the type.
    /// </summary>
    public static OptionTypeDetectionResult Unknown(string source) => new()
    {
        Type = CliOptionType.Unknown,
        Confidence = 0,
        Source = source
    };
}
