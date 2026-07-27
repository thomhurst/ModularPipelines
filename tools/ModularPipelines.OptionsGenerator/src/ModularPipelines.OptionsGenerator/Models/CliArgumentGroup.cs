namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Describes a nested group of CLI arguments declared by help output.
/// </summary>
public record CliArgumentGroup
{
    /// <summary>
    /// Prose introducing the group, including any cardinality constraint.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Semantics inferred from the group prose.
    /// </summary>
    public CliArgumentGroupKind Kind { get; init; }

    /// <summary>
    /// Arguments declared directly in this group.
    /// </summary>
    public IReadOnlyList<CliArgumentDefinition> Arguments { get; init; } = [];

    /// <summary>
    /// Nested argument groups.
    /// </summary>
    public IReadOnlyList<CliArgumentGroup> Groups { get; init; } = [];

    /// <summary>
    /// Traverses all nested groups and carries their prose into each argument's documentation.
    /// </summary>
    public IEnumerable<CliArgumentDefinition> FlattenArguments() =>
        FlattenArguments([])
            .OrderBy(argument => argument.SourceOrder);

    private IEnumerable<CliArgumentDefinition> FlattenArguments(IReadOnlyList<string> parentDescriptions)
    {
        var descriptions = string.IsNullOrWhiteSpace(Description)
            ? parentDescriptions
            : [.. parentDescriptions, Description];
        var groupDescription = descriptions.Count == 0
            ? null
            : string.Join(" ", descriptions);

        foreach (var argument in Arguments)
        {
            yield return argument with { GroupDescription = groupDescription };
        }

        foreach (var group in Groups)
        {
            foreach (var argument in group.FlattenArguments(descriptions))
            {
                yield return argument;
            }
        }
    }
}

/// <summary>
/// A CLI argument declaration before tool-specific type inference.
/// </summary>
public record CliArgumentDefinition
{
    /// <summary>
    /// Normalized switch name, including its leading dashes.
    /// </summary>
    public required string SwitchName { get; init; }

    /// <summary>
    /// Value placeholder shown by help output.
    /// </summary>
    public string? ValueHint { get; init; }

    /// <summary>
    /// Documentation directly beneath the declaration.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Documentation inherited from containing argument groups.
    /// </summary>
    public string? GroupDescription { get; init; }

    /// <summary>
    /// Whether the source syntax represents both positive and negative forms.
    /// </summary>
    public bool IsNegatable { get; init; }

    /// <summary>
    /// Source indentation used to recover nesting.
    /// </summary>
    public int Indentation { get; init; }

    /// <summary>
    /// Source line order retained while nested groups are flattened.
    /// </summary>
    public int SourceOrder { get; init; }

    /// <summary>
    /// Complete documentation retained for generated output.
    /// </summary>
    public string? Documentation => JoinDocumentation(GroupDescription, Description);

    private static string? JoinDocumentation(string? groupDescription, string? description)
    {
        if (string.IsNullOrWhiteSpace(groupDescription))
        {
            return string.IsNullOrWhiteSpace(description) ? null : description;
        }

        return string.IsNullOrWhiteSpace(description)
            ? groupDescription
            : $"{groupDescription} {description}";
    }
}

/// <summary>
/// Semantics commonly used by nested CLI argument groups.
/// </summary>
[Flags]
public enum CliArgumentGroupKind
{
    None = 0,
    Alternative = 1,
    AtMostOne = 2,
    AtLeastOne = 4,
    Resource = 8,
}
