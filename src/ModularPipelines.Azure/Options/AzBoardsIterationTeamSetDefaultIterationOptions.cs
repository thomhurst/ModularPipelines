using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("boards", "iteration", "team", "set-default-iteration")]
public record AzBoardsIterationTeamSetDefaultIterationOptions(
[property: CliOption("--team")] string Team
) : AzOptions
{
    [CliOption("--default-iteration-macro")]
    public string? DefaultIterationMacro { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}