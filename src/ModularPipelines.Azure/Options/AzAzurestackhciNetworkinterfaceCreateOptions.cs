using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci", "networkinterface", "create")]
public record AzAzurestackhciNetworkinterfaceCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [CommandSwitch("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [CommandSwitch("--gateway")]
    public string? Gateway { get; set; }

    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--ip-allocation-method")]
    public string? IpAllocationMethod { get; set; }

    [CommandSwitch("--ip-configurations")]
    public string? IpConfigurations { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mac-address")]
    public string? MacAddress { get; set; }

    [CommandSwitch("--prefix-length")]
    public string? PrefixLength { get; set; }

    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}