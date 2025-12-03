using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("repos", "policy", "merge-strategy", "update")]
public record AzReposPolicyMergeStrategyUpdateOptions(
[property: CliOption("--id")] string Id
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

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--repository-id")]
    public string? RepositoryId { get; set; }
}