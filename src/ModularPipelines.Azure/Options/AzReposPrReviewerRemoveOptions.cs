using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("repos", "pr", "reviewer", "remove")]
public record AzReposPrReviewerRemoveOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--reviewers")] string Reviewers
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}