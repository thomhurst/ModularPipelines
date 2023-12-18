using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci-vm", "network", "lnet", "create")]
public record AzStackHciVmNetworkLnetCreateOptions(
[property: CommandSwitch("--custom-location")] string CustomLocation,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [CommandSwitch("--gateway")]
    public string? Gateway { get; set; }

    [CommandSwitch("--ip-allocation-method")]
    public string? IpAllocationMethod { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--polling-interval")]
    public string? PollingInterval { get; set; }

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