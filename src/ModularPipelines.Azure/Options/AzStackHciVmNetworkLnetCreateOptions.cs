using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stack-hci-vm", "network", "lnet", "create")]
public record AzStackHciVmNetworkLnetCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliOption("--dns-servers")]
    public string? DnsServers { get; set; }

    [CliOption("--gateway")]
    public string? Gateway { get; set; }

    [CliOption("--ip-allocation-method")]
    public string? IpAllocationMethod { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CliOption("--routes")]
    public string? Routes { get; set; }

    [CliOption("--subnets")]
    public string? Subnets { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vlan")]
    public string? Vlan { get; set; }

    [CliOption("--vm-switch-name")]
    public string? VmSwitchName { get; set; }
}