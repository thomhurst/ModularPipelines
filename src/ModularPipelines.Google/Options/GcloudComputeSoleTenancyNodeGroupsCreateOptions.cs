using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-groups", "create")]
public record GcloudComputeSoleTenancyNodeGroupsCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--node-template")] string NodeTemplate,
[property: CliOption("--target-size")] string TargetSize
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--maintenance-policy")]
    public string? MaintenancePolicy { get; set; }

    [CliOption("--maintenance-window-start-time")]
    public string? MaintenanceWindowStartTime { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

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