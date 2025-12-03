using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "policy-based-routes", "create")]
public record GcloudNetworkConnectivityPolicyBasedRoutesCreateOptions(
[property: CliArgument] string PolicyBasedRoute,
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--destination-range")]
    public string? DestinationRange { get; set; }

    [CliOption("--ip-protocol")]
    public string? IpProtocol { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--protocol-version")]
    public string? ProtocolVersion { get; set; }

    [CliOption("--source-range")]
    public string? SourceRange { get; set; }

    [CliOption("--interconnect-attachment-region")]
    public string? InterconnectAttachmentRegion { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--next-hop-ilb-ip")]
    public string? NextHopIlbIp { get; set; }

    [CliOption("--next-hop-other-routes")]
    public string? NextHopOtherRoutes { get; set; }
}