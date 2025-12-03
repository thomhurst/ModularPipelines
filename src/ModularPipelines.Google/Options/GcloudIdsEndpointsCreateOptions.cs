using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ids", "endpoints", "create")]
public record GcloudIdsEndpointsCreateOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Zone,
[property: CliOption("--network")] string Network,
[property: CliOption("--severity")] string Severity
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-traffic-logs")]
    public bool? EnableTrafficLogs { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--max-wait")]
    public string? MaxWait { get; set; }

    [CliOption("--threat-exceptions")]
    public string[]? ThreatExceptions { get; set; }
}