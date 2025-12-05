using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("site-recovery", "network", "mapping", "create")]
public record AzSiteRecoveryNetworkMappingCreateOptions(
[property: CliOption("--fabric-name")] string FabricName,
[property: CliOption("--name")] string Name,
[property: CliOption("--network-name")] string NetworkName,
[property: CliOption("--recovery-network-id")] string RecoveryNetworkId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--fabric-details")]
    public string? FabricDetails { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--recovery-fabric-name")]
    public string? RecoveryFabricName { get; set; }
}