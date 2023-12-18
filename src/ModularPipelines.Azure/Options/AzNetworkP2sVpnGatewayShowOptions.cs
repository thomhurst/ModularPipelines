using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "p2s-vpn-gateway", "show")]
public record AzNetworkP2sVpnGatewayShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--address-space")]
    public string? AddressSpace { get; set; }

    [CommandSwitch("--associated")]
    public string? Associated { get; set; }

    [CommandSwitch("--associated-inbound-routemap")]
    public string? AssociatedInboundRoutemap { get; set; }

    [CommandSwitch("--associated-outbound-routemap")]
    public string? AssociatedOutboundRoutemap { get; set; }

    [CommandSwitch("--config-name")]
    public string? ConfigName { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--propagated")]
    public string? Propagated { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--scale-unit")]
    public string? ScaleUnit { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vpn-server-config")]
    public string? VpnServerConfig { get; set; }
}