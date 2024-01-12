using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "update-interface")]
public record GcloudComputeRoutersUpdateInterfaceOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--interface-name")] string InterfaceName
) : GcloudOptions
{
    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--mask-length")]
    public string? MaskLength { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--interconnect-attachment")]
    public string? InterconnectAttachment { get; set; }

    [CommandSwitch("--interconnect-attachment-region")]
    public string? InterconnectAttachmentRegion { get; set; }

    [CommandSwitch("--vpn-tunnel")]
    public string? VpnTunnel { get; set; }

    [CommandSwitch("--vpn-tunnel-region")]
    public string? VpnTunnelRegion { get; set; }
}