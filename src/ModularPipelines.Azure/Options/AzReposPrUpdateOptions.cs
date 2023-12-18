using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "pr", "update")]
public record AzReposPrUpdateOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [BooleanCommandSwitch("--auto-complete")]
    public bool? AutoComplete { get; set; }

    [BooleanCommandSwitch("--bypass-policy")]
    public bool? BypassPolicy { get; set; }

    [CommandSwitch("--bypass-policy-reason")]
    public string? BypassPolicyReason { get; set; }

    [BooleanCommandSwitch("--delete-source-branch")]
    public bool? DeleteSourceBranch { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--draft")]
    public bool? Draft { get; set; }

    [CommandSwitch("--merge-commit-message")]
    public string? MergeCommitMessage { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [BooleanCommandSwitch("--squash")]
    public bool? Squash { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [BooleanCommandSwitch("--transition-work-items")]
    public bool? TransitionWorkItems { get; set; }
}

