using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "policy", "comment-required", "update")]
public record AzReposPolicyCommentRequiredUpdateOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
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