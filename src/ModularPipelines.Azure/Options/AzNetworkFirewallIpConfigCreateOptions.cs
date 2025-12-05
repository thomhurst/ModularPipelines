using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "ip-config", "create")]
public record AzNetworkFirewallIpConfigCreateOptions(
[property: CliOption("--firewall-name")] string FirewallName,
[property: CliOption("--name")] string Name,
[property: CliOption("--public-ip-address")] string PublicIpAddress,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--m-name")]
    public string? MName { get; set; }

    [CliOption("--m-public-ip-address")]
    public string? MPublicIpAddress { get; set; }

    [CliOption("--m-vnet-name")]
    public string? MVnetName { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}