using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databoxedge", "bandwidth-schedule", "create")]
public record AzDataboxedgeBandwidthScheduleCreateOptions(
[property: CommandSwitch("--days")] int Days,
[property: CommandSwitch("--device-name")] string DeviceName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--rate-in-mbps")] string RateInMbps,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--start")] string Start,
[property: CommandSwitch("--stop")] string Stop
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}