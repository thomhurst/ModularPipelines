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

    /// <summary>
    /// Orders values by their CLI string rather than by scrape order, so anything that emits or
    /// compares enum values sees the same sequence no matter how a tool happened to print its
    /// allowed values. Case-insensitive alphabetical order keeps case variants adjacent; among
    /// case variants the lowercase spelling sorts first so it claims the plain member name and
    /// the uppercase alias receives the casing suffix. Entries that repeat the same CLI string
    /// are ordered by member name, then description, so the one that survives deduplication
    /// does not depend on scrape order either.
    /// </summary>
    public static IEnumerable<CliEnumValue> OrderValues(IEnumerable<CliEnumValue> values) =>
        values
            .OrderBy(value => value.CliValue, StringComparer.OrdinalIgnoreCase)
            .ThenByDescending(value => value.CliValue, StringComparer.Ordinal)
            .ThenBy(value => value.MemberName, StringComparer.Ordinal)
            .ThenBy(value => value.Description, StringComparer.Ordinal);
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
