using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "policy", "merge-strategy", "create")]
public record AzReposPolicyMergeStrategyCreateOptions(
[property: CliFlag("--blocking")] bool Blocking,
[property: CliOption("--branch")] string Branch,
[property: CliFlag("--enabled")] bool Enabled,
[property: CliOption("--repository-id")] string RepositoryId
) : AzOptions
{
    [CliFlag("--allow-no-fast-forward")]
    public bool? AllowNoFastForward { get; set; }

    [CliFlag("--allow-rebase")]
    public bool? AllowRebase { get; set; }

    [CliFlag("--allow-rebase-merge")]
    public bool? AllowRebaseMerge { get; set; }

    [CliFlag("--allow-squash")]
    public bool? AllowSquash { get; set; }

    [CliOption("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}