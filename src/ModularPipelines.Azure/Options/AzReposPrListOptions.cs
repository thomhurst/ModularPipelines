using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "pr", "list")]
public record AzReposPrListOptions : AzOptions
{
    [CliOption("--creator")]
    public string? Creator { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--include-links")]
    public bool? IncludeLinks { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--reviewer")]
    public string? Reviewer { get; set; }

    [CliOption("--skip")]
    public string? Skip { get; set; }

    [CliOption("--source-branch")]
    public string? SourceBranch { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--target-branch")]
    public string? TargetBranch { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}