using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("repos", "policy", "required-reviewer", "update")]
public record AzReposPolicyRequiredReviewerUpdateOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--blocking")]
    public bool? Blocking { get; set; }

    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--message")]
    public string? Message { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--path-filter")]
    public string? PathFilter { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--repository-id")]
    public string? RepositoryId { get; set; }

    [CliOption("--required-reviewer-ids")]
    public string? RequiredReviewerIds { get; set; }
}