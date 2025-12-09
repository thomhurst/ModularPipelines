using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "clustermanager", "create")]
public record AzNetworkcloudClustermanagerCreateOptions(
[property: CliOption("--cluster-manager-name")] string ClusterManagerName,
[property: CliOption("--fabric-controller-id")] string FabricControllerId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--analytics-workspace-id")]
    public string? AnalyticsWorkspaceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-resource-group-configuration")]
    public string? ManagedResourceGroupConfiguration { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}