using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "pr", "create")]
public record AzReposPrCreateOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--vote")] string Vote
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

    [BooleanCommandSwitch("--open")]
    public bool? Open { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--reviewers")]
    public string? Reviewers { get; set; }

    [CommandSwitch("--source-branch")]
    public string? SourceBranch { get; set; }

    [BooleanCommandSwitch("--squash")]
    public bool? Squash { get; set; }

    [CommandSwitch("--target-branch")]
    public string? TargetBranch { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [BooleanCommandSwitch("--transition-work-items")]
    public bool? TransitionWorkItems { get; set; }

    [CommandSwitch("--work-items")]
    public string? WorkItems { get; set; }
}

