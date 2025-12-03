using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databoxedge", "bandwidth-schedule", "create")]
public record AzDataboxedgeBandwidthScheduleCreateOptions(
[property: CliOption("--days")] int Days,
[property: CliOption("--device-name")] string DeviceName,
[property: CliOption("--name")] string Name,
[property: CliOption("--rate-in-mbps")] string RateInMbps,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--start")] string Start,
[property: CliOption("--stop")] string Stop
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}