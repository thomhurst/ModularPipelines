using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "l3network", "create")]
public record AzNetworkcloudL3networkCreateOptions(
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--l3-isolation-domain-id")] string L3IsolationDomainId,
[property: CliOption("--l3-network-name")] string L3NetworkName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vlan")] string Vlan
) : AzOptions
{
    [CliOption("--interface-name")]
    public string? InterfaceName { get; set; }

    [CliOption("--ip-allocation-type")]
    public string? IpAllocationType { get; set; }

    [CliOption("--ipv4-connected-prefix")]
    public string? Ipv4ConnectedPrefix { get; set; }

    [CliOption("--ipv6-connected-prefix")]
    public string? Ipv6ConnectedPrefix { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}