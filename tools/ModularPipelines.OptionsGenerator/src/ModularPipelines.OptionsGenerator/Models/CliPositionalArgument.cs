using ModularPipelines.Attributes;

namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Represents a positional argument for a CLI command.
/// </summary>
public record CliPositionalArgument
{
    public static IReadOnlyList<CliPositionalArgument> MergeDuplicates(
        IEnumerable<CliPositionalArgument> positionalArguments)
    {
        var merged = positionalArguments
            .GroupBy(argument => argument.PropertyName, StringComparer.OrdinalIgnoreCase)
            .Select(group =>
            {
                var first = group.OrderBy(argument => argument.PositionIndex).First();
                var required = group.Any(argument => argument.IsRequired);
                var collection = group.FirstOrDefault(argument =>
                    argument.CSharpType.StartsWith("IEnumerable<", StringComparison.Ordinal));
                var type = (collection ?? first).CSharpType.TrimEnd('?');
                var associatedOptionSwitches = group
                    .Select(argument => argument.AssociatedOptionSwitch)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                return first with
                {
                    CSharpType = required ? type : $"{type}?",
                    IsRequired = required,
                    IsVariadic = group.Any(argument => argument.IsVariadic)
                                 || collection is not null,
                    PrependOptionTerminator = group.Any(argument =>
                        argument.PrependOptionTerminator),
                    RepeatOptionTerminator = group.Any(argument =>
                        argument.RepeatOptionTerminator),
                    PrependOptionTerminatorIfValueStartsWithDash = group.Any(argument =>
                        argument.PrependOptionTerminatorIfValueStartsWithDash),
                    AssociatedOptionSwitch = associatedOptionSwitches.Count == 1
                        ? associatedOptionSwitches[0]
                        : null,
                };
            })
            .OrderBy(argument => argument.PositionIndex)
            .ToList();

        var nextPositions = new Dictionary<CommandLinePhase, int>();
        return merged
            .Select(argument =>
            {
                var position = nextPositions.GetValueOrDefault(argument.Phase);
                nextPositions[argument.Phase] = position + 1;
                return argument with { PositionIndex = position };
            })
            .ToList();
    }

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
    /// Option switch that syntactically owns this placeholder in the usage synopsis.
    /// </summary>
    public string? AssociatedOptionSwitch { get; init; }

    /// <summary>
    /// Rendering phase relative to flags and options. Generated operands default to
    /// <see cref="CommandLinePhase.EarlyOperand"/> because scraper syntax places them beside
    /// the command path; hand-written attributes retain their pass-through default for compatibility.
    /// </summary>
    public CommandLinePhase Phase { get; init; } = CommandLinePhase.EarlyOperand;

    /// <summary>
    /// Zero-based position index among positional arguments in the same phase.
    /// </summary>
    public int PositionIndex { get; init; }

    /// <summary>
    /// Whether this positional argument is required.
    /// </summary>
    public bool IsRequired { get; init; }

    /// <summary>
    /// Whether this positional argument accepts repeated values.
    /// </summary>
    public bool IsVariadic { get; init; }

    /// <summary>
    /// Whether the generated CLI argument must be preceded by the <c>--</c> option terminator.
    /// </summary>
    public bool PrependOptionTerminator { get; init; }

    /// <summary>
    /// Whether the generated CLI argument needs a fresh option terminator after an earlier one.
    /// </summary>
    public bool RepeatOptionTerminator { get; init; }

    /// <summary>
    /// Whether a dash-prefixed generated CLI argument must be preceded by the <c>--</c>
    /// option terminator.
    /// </summary>
    public bool PrependOptionTerminatorIfValueStartsWithDash { get; init; }

    /// <summary>
    /// Allows an intentional rendering-phase migration for an existing generated operand.
    /// The compatibility guard still requires the same property name and type. Position may
    /// change because positional indexes are scoped to a rendering phase.
    /// </summary>
    public bool AllowRenderingPhaseMigrationFromBaseline { get; init; }

    /// <summary>
    /// Whether this positional argument contains a secret value that should be obfuscated in logs.
    /// </summary>
    public bool IsSecret { get; init; }
}
