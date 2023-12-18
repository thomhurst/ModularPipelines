using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "policy", "merge-strategy", "update")]
public record AzReposPolicyMergeStrategyUpdateOptions(
[property: CommandSwitch("--id")] string Id
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

    [BooleanCommandSwitch("--blocking")]
    public bool? Blocking { get; set; }

    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [CommandSwitch("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--repository-id")]
    public string? RepositoryId { get; set; }
}