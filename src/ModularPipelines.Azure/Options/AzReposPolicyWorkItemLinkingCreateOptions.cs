using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "policy", "work-item-linking", "create")]
public record AzReposPolicyWorkItemLinkingCreateOptions(
[property: CliFlag("--blocking")] bool Blocking,
[property: CliOption("--branch")] string Branch,
[property: CliFlag("--enabled")] bool Enabled,
[property: CliOption("--repository-id")] string RepositoryId
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