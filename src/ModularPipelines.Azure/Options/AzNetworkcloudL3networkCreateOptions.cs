using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "l3network", "create")]
public record AzNetworkcloudL3networkCreateOptions(
[property: CommandSwitch("--extended-location")] string ExtendedLocation,
[property: CommandSwitch("--l3-isolation-domain-id")] string L3IsolationDomainId,
[property: CommandSwitch("--l3-network-name")] string L3NetworkName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vlan")] string Vlan
) : AzOptions
{
    [CommandSwitch("--interface-name")]
    public string? InterfaceName { get; set; }

    [CommandSwitch("--ip-allocation-type")]
    public string? IpAllocationType { get; set; }

    [CommandSwitch("--ipv4-connected-prefix")]
    public string? Ipv4ConnectedPrefix { get; set; }

    [CommandSwitch("--ipv6-connected-prefix")]
    public string? Ipv6ConnectedPrefix { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}