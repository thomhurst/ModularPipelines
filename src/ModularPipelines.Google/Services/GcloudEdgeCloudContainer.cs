using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("edge-cloud")]
public class GcloudEdgeCloudContainer
{
    public GcloudEdgeCloudContainer(
        GcloudEdgeCloudContainerClusters clusters,
        GcloudEdgeCloudContainerMachines machines,
        GcloudEdgeCloudContainerOperations operations,
        GcloudEdgeCloudContainerRegions regions,
        GcloudEdgeCloudContainerVpnConnections vpnConnections,
        GcloudEdgeCloudContainerZones zones,
        ICommand internalCommand
    )
    {
        Clusters = clusters;
        Machines = machines;
        Operations = operations;
        Regions = regions;
        VpnConnections = vpnConnections;
        Zones = zones;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudEdgeCloudContainerClusters Clusters { get; }

    public GcloudEdgeCloudContainerMachines Machines { get; }

    public GcloudEdgeCloudContainerOperations Operations { get; }

    public GcloudEdgeCloudContainerRegions Regions { get; }

    public GcloudEdgeCloudContainerVpnConnections VpnConnections { get; }

    public GcloudEdgeCloudContainerZones Zones { get; }

    public async Task<CommandResult> GetServerConfig(GcloudEdgeCloudContainerGetServerConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudEdgeCloudContainerGetServerConfigOptions(), token);
    }
}