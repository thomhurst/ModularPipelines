namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Represents an enum to be generated for constrained option values.
/// </summary>
public record CliEnumDefinition
{
    /// <summary>
    /// The enum name (e.g., "DockerBuildOutputFormat").
    /// </summary>
    public required string EnumName { get; init; }

    /// <summary>
    /// The possible values for this enum.
    /// </summary>
    public required IReadOnlyList<CliEnumValue> Values { get; init; }

    /// <summary>
    /// Description for XML documentation.
    /// </summary>
    public string? Description { get; init; }
}

/// <summary>
/// Represents a single enum value.
/// </summary>
public record CliEnumValue
{
    /// <summary>
    /// The C# enum member name (PascalCase).
    /// </summary>
    public required string MemberName { get; init; }

    /// <summary>
    /// The CLI string value (e.g., "json", "yaml").
    /// </summary>
    public required string CliValue { get; init; }

    /// <summary>
    /// Description for XML documentation.
    /// </summary>
    public string? Description { get; init; }
}
