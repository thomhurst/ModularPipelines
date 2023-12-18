using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "clustermanager", "create")]
public record AzNetworkcloudClustermanagerCreateOptions(
[property: CommandSwitch("--cluster-manager-name")] string ClusterManagerName,
[property: CommandSwitch("--fabric-controller-id")] string FabricControllerId,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--analytics-workspace-id")]
    public string? AnalyticsWorkspaceId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-resource-group-configuration")]
    public string? ManagedResourceGroupConfiguration { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}