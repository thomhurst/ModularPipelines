using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-dns", "link", "vnet", "create")]
public record AzNetworkPrivateDnsLinkVnetCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliFlag("--registration-enabled")] bool RegistrationEnabled,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--virtual-network")] string VirtualNetwork,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}