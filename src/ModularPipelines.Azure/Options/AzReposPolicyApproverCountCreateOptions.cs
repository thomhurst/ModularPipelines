using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "policy", "approver-count", "create")]
public record AzReposPolicyApproverCountCreateOptions(
[property: BooleanCommandSwitch("--allow-downvotes")] bool AllowDownvotes,
[property: BooleanCommandSwitch("--blocking")] bool Blocking,
[property: CommandSwitch("--branch")] string Branch,
[property: BooleanCommandSwitch("--creator-vote-counts")] bool CreatorVoteCounts,
[property: BooleanCommandSwitch("--enabled")] bool Enabled,
[property: CommandSwitch("--minimum-approver-count")] int MinimumApproverCount,
[property: CommandSwitch("--repository-id")] string RepositoryId,
[property: BooleanCommandSwitch("--reset-on-source-push")] bool ResetOnSourcePush
) : AzOptions
{
    [CommandSwitch("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}