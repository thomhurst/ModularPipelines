using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("urestackhci", "networkinterface", "create")]
public record AzAzurestackhciNetworkinterfaceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--dns-servers")]
    public string? DnsServers { get; set; }

    [CliOption("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [CliOption("--gateway")]
    public string? Gateway { get; set; }

    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--ip-allocation-method")]
    public string? IpAllocationMethod { get; set; }

    [CliOption("--ip-configurations")]
    public string? IpConfigurations { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mac-address")]
    public string? MacAddress { get; set; }

    [CliOption("--prefix-length")]
    public string? PrefixLength { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}