using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "check")]
public record AzIotOpsCheckOptions : AzOptions
{
    [BooleanCommandSwitch("--as-object")]
    public bool? AsObject { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--detail-level")]
    public string? DetailLevel { get; set; }

    [CommandSwitch("--ops-service")]
    public string? OpsService { get; set; }

    [BooleanCommandSwitch("--post")]
    public bool? Post { get; set; }

    [BooleanCommandSwitch("--pre")]
    public bool? Pre { get; set; }

    [CommandSwitch("--resources")]
    public string? Resources { get; set; }
}