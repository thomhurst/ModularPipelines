using ModularPipelines.Attributes;

namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// A command-level constraint requiring at least one generated option or operand.
/// </summary>
public sealed record CliRequiredAlternativeGroup
{
    /// <summary>
    /// Generated members participating in the choice.
    /// </summary>
    public required IReadOnlyList<CliRequiredAlternativeMember> Members { get; init; }

    /// <summary>
    /// Generated property names participating in the choice.
    /// </summary>
    public IReadOnlyList<string> PropertyNames => Members.Select(static member => member.PropertyName).ToArray();
}

/// <summary>
/// A generated option or positional argument participating in a required choice.
/// </summary>
public sealed record CliRequiredAlternativeMember
{
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
