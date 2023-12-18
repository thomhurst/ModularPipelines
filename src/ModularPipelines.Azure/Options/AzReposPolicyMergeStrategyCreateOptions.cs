using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "policy", "merge-strategy", "create")]
public record AzReposPolicyMergeStrategyCreateOptions(
[property: BooleanCommandSwitch("--blocking")] bool Blocking,
[property: CommandSwitch("--branch")] string Branch,
[property: BooleanCommandSwitch("--enabled")] bool Enabled,
[property: CommandSwitch("--repository-id")] string RepositoryId
) : AzOptions
{
    [BooleanCommandSwitch("--allow-no-fast-forward")]
    public bool? AllowNoFastForward { get; set; }

    [BooleanCommandSwitch("--allow-rebase")]
    public bool? AllowRebase { get; set; }

    [BooleanCommandSwitch("--allow-rebase-merge")]
    public bool? AllowRebaseMerge { get; set; }

    [BooleanCommandSwitch("--allow-squash")]
    public bool? AllowSquash { get; set; }

    [CommandSwitch("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}