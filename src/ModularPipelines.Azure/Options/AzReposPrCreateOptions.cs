using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "pr", "create")]
public record AzReposPrCreateOptions : AzOptions
{
    [CliFlag("--auto-complete")]
    public bool? AutoComplete { get; set; }

    [CliFlag("--bypass-policy")]
    public bool? BypassPolicy { get; set; }

    [CliOption("--bypass-policy-reason")]
    public string? BypassPolicyReason { get; set; }

    [CliFlag("--delete-source-branch")]
    public bool? DeleteSourceBranch { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--draft")]
    public bool? Draft { get; set; }

    [CliOption("--merge-commit-message")]
    public string? MergeCommitMessage { get; set; }

    [CliFlag("--open")]
    public bool? Open { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--reviewers")]
    public string? Reviewers { get; set; }

    [CliOption("--source-branch")]
    public string? SourceBranch { get; set; }

    [CliFlag("--squash")]
    public bool? Squash { get; set; }

    [CliOption("--target-branch")]
    public string? TargetBranch { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliFlag("--transition-work-items")]
    public bool? TransitionWorkItems { get; set; }

    [CliOption("--work-items")]
    public string? WorkItems { get; set; }
}