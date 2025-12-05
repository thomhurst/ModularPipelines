using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "update-interface")]
public record GcloudComputeRoutersUpdateInterfaceOptions(
[property: CliArgument] string Name,
[property: CliOption("--interface-name")] string InterfaceName
) : GcloudOptions
{
    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--mask-length")]
    public string? MaskLength { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--interconnect-attachment")]
    public string? InterconnectAttachment { get; set; }

    [CliOption("--interconnect-attachment-region")]
    public string? InterconnectAttachmentRegion { get; set; }

    [CliOption("--vpn-tunnel")]
    public string? VpnTunnel { get; set; }

    [CliOption("--vpn-tunnel-region")]
    public string? VpnTunnelRegion { get; set; }
}