using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "p2s-vpn-gateway", "update")]
public record AzNetworkP2sVpnGatewayUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

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

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--propagated")]
    public string? Propagated { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--scale-unit")]
    public string? ScaleUnit { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vpn-server-config")]
    public string? VpnServerConfig { get; set; }
}