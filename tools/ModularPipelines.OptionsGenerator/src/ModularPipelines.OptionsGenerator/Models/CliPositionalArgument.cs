namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Represents a positional argument for a CLI command.
/// </summary>
public record CliPositionalArgument
{
    /// <summary>
    /// Placeholder name in documentation (e.g., "[<PROJECT>]").
    /// </summary>
    public string? PlaceholderName { get; init; }

    /// <summary>
    /// Generated property name.
    /// </summary>
    public required string PropertyName { get; init; }

    /// <summary>
    /// C# type.
    /// </summary>
    public required string CSharpType { get; init; }

    /// <summary>
    /// Description for XML documentation.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Placement relative to options.
    /// </summary>
    public PositionalArgumentPosition Placement { get; init; } = PositionalArgumentPosition.AfterOptions;

    /// <summary>
    /// Zero-based position index among positional arguments with the same placement.
    /// </summary>
    public int PositionIndex { get; init; }

    /// <summary>
    /// Whether this positional argument is required.
    /// </summary>
    public bool IsRequired { get; init; }
}

/// <summary>
/// Position of the argument relative to options.
/// Maps to ModularPipelines.Attributes.ArgumentPlacement.
/// </summary>
public enum PositionalArgumentPosition
{
    /// <summary>
    /// Argument is placed after all options and flags (default).
    /// </summary>
    AfterOptions,

    /// <summary>
    /// Argument is placed before any options and flags.
    /// </summary>
    BeforeOptions,

    /// <summary>
    /// Argument is placed immediately after the command, before options.
    /// </summary>
    ImmediatelyAfterCommand,

    // Legacy aliases for backwards compatibility
    BeforeSwitches = BeforeOptions,
    AfterSwitches = AfterOptions,
}
