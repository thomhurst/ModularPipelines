using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databoxedge", "bandwidth-schedule", "show")]
public record AzDataboxedgeBandwidthScheduleShowOptions(
[property: CommandSwitch("--days")] int Days,
[property: CommandSwitch("--rate-in-mbps")] string RateInMbps,
[property: CommandSwitch("--start")] string Start,
[property: CommandSwitch("--stop")] string Stop
) : AzOptions
{
    [CommandSwitch("--device-name")]
    public string? DeviceName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}