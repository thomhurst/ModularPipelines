namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Context passed to type detectors containing information about the option being analyzed.
/// </summary>
public class OptionDetectionContext
{
    /// <summary>
    /// The name of the option (e.g., "--name", "--detach").
    /// </summary>
    public required string OptionName { get; init; }

    /// <summary>
    /// All names/aliases for this option (e.g., ["-d", "--detach"]).
    /// </summary>
    public required IReadOnlyList<string> AllNames { get; init; }

    /// <summary>
    /// Description of the option from documentation.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Default value from documentation.
    /// </summary>
    public string? DefaultValue { get; init; }

    /// <summary>
    /// Accepted values from documentation (e.g., "true, false, yes, no").
    /// </summary>
    public string? AcceptedValues { get; init; }

    /// <summary>
    /// The CLI tool name (e.g., "docker", "kubectl", "az").
    /// </summary>
    public required string ToolName { get; init; }

    /// <summary>
    /// The full command path (e.g., ["docker", "container", "run"]).
    /// </summary>
    public required string[] CommandPath { get; init; }

    /// <summary>
    /// Cache for sharing expensive-to-generate data across all options for the same command.
    /// Detectors can store parsed help text, etc. here to avoid redundant work.
    /// When provided externally, allows cache reuse across multiple options.
    /// </summary>
    public IDictionary<object, object> CommandCache { get; init; } = new Dictionary<object, object>();
}
