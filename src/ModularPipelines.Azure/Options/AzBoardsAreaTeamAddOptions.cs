using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("boards", "area", "team", "add")]
public record AzBoardsAreaTeamAddOptions(
[property: CliOption("--path")] string Path,
[property: CliOption("--team")] string Team
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--include-sub-areas")]
    public bool? IncludeSubAreas { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliFlag("--set-as-default")]
    public bool? SetAsDefault { get; set; }
}