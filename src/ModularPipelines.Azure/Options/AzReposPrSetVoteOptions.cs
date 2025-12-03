using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("repos", "pr", "set-vote")]
public record AzReposPrSetVoteOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--vote")] string Vote
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}