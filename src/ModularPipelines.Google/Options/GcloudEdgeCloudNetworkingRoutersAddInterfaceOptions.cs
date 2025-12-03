using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "routers", "add-interface")]
public record GcloudEdgeCloudNetworkingRoutersAddInterfaceOptions(
[property: CliArgument] string Router,
[property: CliArgument] string Location,
[property: CliArgument] string Zone,
[property: CliOption("--interface-name")] string InterfaceName,
[property: CliOption("--loopback-ip-addresses")] string[] LoopbackIpAddresses,
[property: CliOption("--subnetwork")] string Subnetwork,
[property: CliOption("--interconnect-attachment")] string InterconnectAttachment,
[property: CliOption("--ip-address")] string IpAddress,
[property: CliOption("--ip-mask-length")] string IpMaskLength
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}