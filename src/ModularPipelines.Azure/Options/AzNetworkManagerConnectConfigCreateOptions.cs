using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "connect-config", "create")]
public record AzNetworkManagerConnectConfigCreateOptions(
[property: CommandSwitch("--applies-to-groups")] string AppliesToGroups,
[property: CommandSwitch("--configuration-name")] string ConfigurationName,
[property: CommandSwitch("--connectivity-topology")] string ConnectivityTopology,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--delete-existing-peering")]
    public bool? DeleteExistingPeering { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--hub")]
    public string? Hub { get; set; }

    [BooleanCommandSwitch("--is-global")]
    public bool? IsGlobal { get; set; }
}