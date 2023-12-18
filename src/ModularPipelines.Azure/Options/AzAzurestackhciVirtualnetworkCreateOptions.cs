using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci", "virtualnetwork", "create")]
public record AzAzurestackhciVirtualnetworkCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [CommandSwitch("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [CommandSwitch("--gateway")]
    public string? Gateway { get; set; }

    [CommandSwitch("--ip-allocation-method")]
    public string? IpAllocationMethod { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--routes")]
    public string? Routes { get; set; }

    [CommandSwitch("--subnets")]
    public string? Subnets { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vlan")]
    public string? Vlan { get; set; }

    [CommandSwitch("--vm-switch-name")]
    public string? VmSwitchName { get; set; }
}

