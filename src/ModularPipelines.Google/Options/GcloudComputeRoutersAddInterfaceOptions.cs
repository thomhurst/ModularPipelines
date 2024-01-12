using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "add-interface")]
public record GcloudComputeRoutersAddInterfaceOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--interface-name")] string InterfaceName,
[property: CommandSwitch("--interconnect-attachment")] string InterconnectAttachment,
[property: CommandSwitch("--interconnect-attachment-region")] string InterconnectAttachmentRegion,
[property: CommandSwitch("--subnetwork")] string Subnetwork,
[property: CommandSwitch("--subnetwork-region")] string SubnetworkRegion,
[property: CommandSwitch("--vpn-tunnel")] string VpnTunnel,
[property: CommandSwitch("--vpn-tunnel-region")] string VpnTunnelRegion
) : GcloudOptions
{
    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--mask-length")]
    public string? MaskLength { get; set; }

    [CommandSwitch("--redundant-interface")]
    public string? RedundantInterface { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}