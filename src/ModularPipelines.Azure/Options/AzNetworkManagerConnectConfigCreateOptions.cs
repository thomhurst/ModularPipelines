using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "connect-config", "create")]
public record AzNetworkManagerConnectConfigCreateOptions(
[property: CliOption("--applies-to-groups")] string AppliesToGroups,
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--connectivity-topology")] string ConnectivityTopology,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--delete-existing-peering")]
    public bool? DeleteExistingPeering { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--hub")]
    public string? Hub { get; set; }

    [CliFlag("--is-global")]
    public bool? IsGlobal { get; set; }
}