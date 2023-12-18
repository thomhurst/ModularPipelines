using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "network", "mapping", "create")]
public record AzSiteRecoveryNetworkMappingCreateOptions(
[property: CommandSwitch("--fabric-name")] string FabricName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--network-name")] string NetworkName,
[property: CommandSwitch("--recovery-network-id")] string RecoveryNetworkId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--fabric-details")]
    public string? FabricDetails { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--recovery-fabric-name")]
    public string? RecoveryFabricName { get; set; }
}