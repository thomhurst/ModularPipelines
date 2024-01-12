using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "routers", "add-interface")]
public record GcloudEdgeCloudNetworkingRoutersAddInterfaceOptions(
[property: PositionalArgument] string Router,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone,
[property: CommandSwitch("--interface-name")] string InterfaceName,
[property: CommandSwitch("--loopback-ip-addresses")] string[] LoopbackIpAddresses,
[property: CommandSwitch("--subnetwork")] string Subnetwork,
[property: CommandSwitch("--interconnect-attachment")] string InterconnectAttachment,
[property: CommandSwitch("--ip-address")] string IpAddress,
[property: CommandSwitch("--ip-mask-length")] string IpMaskLength
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}