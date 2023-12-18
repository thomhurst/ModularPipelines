using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "policy", "required-reviewer", "create")]
public record AzReposPolicyRequiredReviewerCreateOptions(
[property: BooleanCommandSwitch("--blocking")] bool Blocking,
[property: CommandSwitch("--branch")] string Branch,
[property: BooleanCommandSwitch("--enabled")] bool Enabled,
[property: CommandSwitch("--message")] string Message,
[property: CommandSwitch("--repository-id")] string RepositoryId,
[property: CommandSwitch("--required-reviewer-ids")] string RequiredReviewerIds
) : AzOptions
{
    [CommandSwitch("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--path-filter")]
    public string? PathFilter { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}