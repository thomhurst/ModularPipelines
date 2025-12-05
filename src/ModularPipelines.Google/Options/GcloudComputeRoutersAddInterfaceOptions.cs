using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "add-interface")]
public record GcloudComputeRoutersAddInterfaceOptions(
[property: CliArgument] string Name,
[property: CliOption("--interface-name")] string InterfaceName,
[property: CliOption("--interconnect-attachment")] string InterconnectAttachment,
[property: CliOption("--interconnect-attachment-region")] string InterconnectAttachmentRegion,
[property: CliOption("--subnetwork")] string Subnetwork,
[property: CliOption("--subnetwork-region")] string SubnetworkRegion,
[property: CliOption("--vpn-tunnel")] string VpnTunnel,
[property: CliOption("--vpn-tunnel-region")] string VpnTunnelRegion
) : GcloudOptions
{
    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--mask-length")]
    public string? MaskLength { get; set; }

    [CliOption("--redundant-interface")]
    public string? RedundantInterface { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}