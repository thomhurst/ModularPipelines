using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-groups", "create")]
public record GcloudComputeSoleTenancyNodeGroupsCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--node-template")] string NodeTemplate,
[property: CommandSwitch("--target-size")] string TargetSize
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--maintenance-policy")]
    public string? MaintenancePolicy { get; set; }

    [CommandSwitch("--maintenance-window-start-time")]
    public string? MaintenanceWindowStartTime { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--autoscaler-mode")]
    public string? AutoscalerMode { get; set; }

    [BooleanCommandSwitch("off")]
    public bool? Off { get; set; }

    [BooleanCommandSwitch("on")]
    public bool? On { get; set; }

    [BooleanCommandSwitch("only-scale-out")]
    public bool? OnlyScaleOut { get; set; }

    [CommandSwitch("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CommandSwitch("--min-nodes")]
    public string? MinNodes { get; set; }

    [CommandSwitch("--share-setting")]
    public string? ShareSetting { get; set; }

    [CommandSwitch("--share-with")]
    public string[]? ShareWith { get; set; }
}