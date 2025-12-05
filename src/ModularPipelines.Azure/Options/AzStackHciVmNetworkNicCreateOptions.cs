using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack-hci-vm", "network", "nic", "create")]
public record AzStackHciVmNetworkNicCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--dns-servers")]
    public string? DnsServers { get; set; }

    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--ip-configurations")]
    public string? IpConfigurations { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mac-address")]
    public string? MacAddress { get; set; }

    [CliOption("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}