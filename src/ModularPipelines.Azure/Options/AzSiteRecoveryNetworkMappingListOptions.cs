using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "network", "mapping", "list")]
public record AzSiteRecoveryNetworkMappingListOptions(
[property: CommandSwitch("--fabric-name")] string FabricName,
[property: CommandSwitch("--network-name")] string NetworkName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }
}