using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("repos", "policy", "approver-count", "create")]
public record AzReposPolicyApproverCountCreateOptions(
[property: CliFlag("--allow-downvotes")] bool AllowDownvotes,
[property: CliFlag("--blocking")] bool Blocking,
[property: CliOption("--branch")] string Branch,
[property: CliFlag("--creator-vote-counts")] bool CreatorVoteCounts,
[property: CliFlag("--enabled")] bool Enabled,
[property: CliOption("--minimum-approver-count")] int MinimumApproverCount,
[property: CliOption("--repository-id")] string RepositoryId,
[property: CliFlag("--reset-on-source-push")] bool ResetOnSourcePush
) : AzOptions
{
    [CliOption("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}