using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "p2s-vpn-gateway", "create")]
public record AzNetworkP2sVpnGatewayCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--scale-unit")] string ScaleUnit,
[property: CommandSwitch("--vhub")] string Vhub
) : AzOptions
{
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

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--propagated")]
    public string? Propagated { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vpn-server-config")]
    public string? VpnServerConfig { get; set; }
}

