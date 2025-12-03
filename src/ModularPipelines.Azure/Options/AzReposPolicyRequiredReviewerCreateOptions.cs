using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("repos", "policy", "required-reviewer", "create")]
public record AzReposPolicyRequiredReviewerCreateOptions(
[property: CliFlag("--blocking")] bool Blocking,
[property: CliOption("--branch")] string Branch,
[property: CliFlag("--enabled")] bool Enabled,
[property: CliOption("--message")] string Message,
[property: CliOption("--repository-id")] string RepositoryId,
[property: CliOption("--required-reviewer-ids")] string RequiredReviewerIds
) : AzOptions
{
    [CliOption("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--path-filter")]
    public string? PathFilter { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}