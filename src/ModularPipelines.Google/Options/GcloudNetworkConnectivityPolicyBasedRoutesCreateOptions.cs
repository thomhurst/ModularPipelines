using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "policy-based-routes", "create")]
public record GcloudNetworkConnectivityPolicyBasedRoutesCreateOptions(
[property: PositionalArgument] string PolicyBasedRoute,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--destination-range")]
    public string? DestinationRange { get; set; }

    [CommandSwitch("--ip-protocol")]
    public string? IpProtocol { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--protocol-version")]
    public string? ProtocolVersion { get; set; }

    [CommandSwitch("--source-range")]
    public string? SourceRange { get; set; }

    [CommandSwitch("--interconnect-attachment-region")]
    public string? InterconnectAttachmentRegion { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--next-hop-ilb-ip")]
    public string? NextHopIlbIp { get; set; }

    [CommandSwitch("--next-hop-other-routes")]
    public string? NextHopOtherRoutes { get; set; }
}