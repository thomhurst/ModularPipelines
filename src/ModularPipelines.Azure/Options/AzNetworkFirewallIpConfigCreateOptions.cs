using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "ip-config", "create")]
public record AzNetworkFirewallIpConfigCreateOptions(
[property: CommandSwitch("--firewall-name")] string FirewallName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--public-ip-address")] string PublicIpAddress,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--m-name")]
    public string? MName { get; set; }

    [CommandSwitch("--m-public-ip-address")]
    public string? MPublicIpAddress { get; set; }

    [CommandSwitch("--m-vnet-name")]
    public string? MVnetName { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}