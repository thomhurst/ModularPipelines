using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-groups", "update")]
public record GcloudComputeSoleTenancyNodeGroupsUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--node-template")]
    public string? NodeTemplate { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--add-nodes")]
    public string? AddNodes { get; set; }

    [CommandSwitch("--delete-nodes")]
    public string[]? DeleteNodes { get; set; }

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