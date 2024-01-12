using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ids", "endpoints", "create")]
public record GcloudIdsEndpointsCreateOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Zone,
[property: CommandSwitch("--network")] string Network,
[property: CommandSwitch("--severity")] string Severity
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-traffic-logs")]
    public bool? EnableTrafficLogs { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--max-wait")]
    public string? MaxWait { get; set; }

    [CommandSwitch("--threat-exceptions")]
    public string[]? ThreatExceptions { get; set; }
}