using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "monitor-events")]
public record AzIotHubMonitorEventsOptions(
[property: CommandSwitch("--query-command")] string QueryCommand
) : AzOptions
{
    [CommandSwitch("--cg")]
    public string? Cg { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--device-id")]
    public string? DeviceId { get; set; }

    [CommandSwitch("--device-query")]
    public string? DeviceQuery { get; set; }

    [CommandSwitch("--enqueued-time")]
    public string? EnqueuedTime { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--interface")]
    public string? Interface { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--mc")]
    public string? Mc { get; set; }

    [CommandSwitch("--module-id")]
    public string? ModuleId { get; set; }

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [BooleanCommandSwitch("--repair")]
    public bool? Repair { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}