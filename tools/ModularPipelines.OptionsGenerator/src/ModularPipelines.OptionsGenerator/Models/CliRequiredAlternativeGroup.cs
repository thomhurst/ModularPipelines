namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// A command-level constraint requiring at least one generated option or operand.
/// </summary>
public sealed record CliRequiredAlternativeGroup
{
    /// <summary>
    /// Generated property names participating in the choice.
    /// </summary>
    public required IReadOnlyList<string> PropertyNames { get; init; }
}
