using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "p2s-vpn-gateway", "create")]
public record AzNetworkP2sVpnGatewayCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--scale-unit")] string ScaleUnit,
[property: CliOption("--vhub")] string Vhub
) : AzOptions
{
    [CliOption("--address-space")]
    public string? AddressSpace { get; set; }

    [CliOption("--associated")]
    public string? Associated { get; set; }

    [CliOption("--associated-inbound-routemap")]
    public string? AssociatedInboundRoutemap { get; set; }

    [CliOption("--associated-outbound-routemap")]
    public string? AssociatedOutboundRoutemap { get; set; }

    [CliOption("--config-name")]
    public string? ConfigName { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--propagated")]
    public string? Propagated { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vpn-server-config")]
    public string? VpnServerConfig { get; set; }
}