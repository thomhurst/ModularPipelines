using ModularPipelines.Attributes;

namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// A presence constraint over generated options, operands, and nested branches.
/// </summary>
public sealed record CliRequiredAlternativeGroup
{
    /// <summary>
    /// Whether the group must be present when its containing bundle is selected.
    /// </summary>
    public bool IsRequired { get; init; } = true;

    /// <summary>
    /// Whether members and nested groups are alternatives rather than one argument bundle.
    /// </summary>
    public bool IsChoice { get; init; } = true;

    /// <summary>
    /// Whether supplying more than one member is also invalid.
    /// </summary>
    public bool IsMutuallyExclusive { get; init; }

    /// <summary>
    /// Generated members participating in the choice.
    /// </summary>
    public required IReadOnlyList<CliRequiredAlternativeMember> Members { get; init; }

    /// <summary>
    /// Nested branches whose presence counts once in a containing choice.
    /// </summary>
    public IReadOnlyList<CliRequiredAlternativeGroup> Groups { get; init; } = [];

    /// <summary>
    /// Generated property names participating in the choice.
    /// </summary>
    public IReadOnlyList<string> PropertyNames =>
        [.. Members.Select(static member => member.PropertyName).Concat(Groups.SelectMany(static group => group.PropertyNames))];
}

/// <summary>
/// A generated option or positional argument participating in a required choice.
/// </summary>
public sealed record CliRequiredAlternativeMember
{
    /// <summary>
    /// Whether this member must be present when its containing argument bundle is selected.
    /// </summary>
    public bool IsRequired { get; init; }

    /// <summary>
    /// Current generated property name.
    /// </summary>
    public required string PropertyName { get; init; }

    /// <summary>
    /// Stable CLI switch identity, when this member is an option.
    /// </summary>
    public string? OptionSwitch { get; init; }

    /// <summary>
    /// Stable rendering phase identity, when this member is an operand.
    /// </summary>
    public CommandLinePhase? PositionalArgumentPhase { get; init; }

    /// <summary>
    /// Stable position within the rendering phase, when this member is an operand.
    /// </summary>
    public int? PositionalArgumentPositionIndex { get; init; }
}
