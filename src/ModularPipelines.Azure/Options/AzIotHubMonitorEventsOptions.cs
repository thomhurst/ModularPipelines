using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "monitor-events")]
public record AzIotHubMonitorEventsOptions : AzOptions
{
    [CliOption("--cg")]
    public string? Cg { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--device-id")]
    public string? DeviceId { get; set; }

    [CliOption("--device-query")]
    public string? DeviceQuery { get; set; }

    [CliOption("--enqueued-time")]
    public string? EnqueuedTime { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--interface")]
    public string? Interface { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--mc")]
    public string? Mc { get; set; }

    [CliOption("--module-id")]
    public string? ModuleId { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliFlag("--repair")]
    public bool? Repair { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}