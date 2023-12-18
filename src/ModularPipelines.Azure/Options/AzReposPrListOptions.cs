using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "pr", "list")]
public record AzReposPrListOptions : AzOptions
{
    [CommandSwitch("--creator")]
    public string? Creator { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--include-links")]
    public bool? IncludeLinks { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--reviewer")]
    public string? Reviewer { get; set; }

    [CommandSwitch("--skip")]
    public string? Skip { get; set; }

    [CommandSwitch("--source-branch")]
    public string? SourceBranch { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--target-branch")]
    public string? TargetBranch { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}