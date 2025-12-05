using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-groups", "update")]
public record GcloudComputeSoleTenancyNodeGroupsUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--node-template")]
    public string? NodeTemplate { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--add-nodes")]
    public string? AddNodes { get; set; }

    [CliOption("--delete-nodes")]
    public string[]? DeleteNodes { get; set; }

    [CliOption("--autoscaler-mode")]
    public string? AutoscalerMode { get; set; }

    [CliFlag("off")]
    public bool? Off { get; set; }

    [CliFlag("on")]
    public bool? On { get; set; }

    [CliFlag("only-scale-out")]
    public bool? OnlyScaleOut { get; set; }

    [CliOption("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CliOption("--min-nodes")]
    public string? MinNodes { get; set; }

    [CliOption("--share-setting")]
    public string? ShareSetting { get; set; }

    [CliOption("--share-with")]
    public string[]? ShareWith { get; set; }
}