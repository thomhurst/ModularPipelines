using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("databoxedge", "bandwidth-schedule", "update")]
public record AzDataboxedgeBandwidthScheduleUpdateOptions(
[property: CliOption("--days")] int Days,
[property: CliOption("--rate-in-mbps")] string RateInMbps,
[property: CliOption("--start")] string Start,
[property: CliOption("--stop")] string Stop
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--device-name")]
    public string? DeviceName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}