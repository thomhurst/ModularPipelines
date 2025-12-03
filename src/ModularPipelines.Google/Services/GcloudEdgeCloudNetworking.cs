using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud")]
public class GcloudEdgeCloudNetworking
{
    public GcloudEdgeCloudNetworking(
        GcloudEdgeCloudNetworkingInterconnects interconnects,
        GcloudEdgeCloudNetworkingNetworks networks,
        GcloudEdgeCloudNetworkingOperations operations,
        GcloudEdgeCloudNetworkingRouters routers,
        GcloudEdgeCloudNetworkingSubnets subnets,
        GcloudEdgeCloudNetworkingZones zones
    )
    {
        Interconnects = interconnects;
        Networks = networks;
        Operations = operations;
        Routers = routers;
        Subnets = subnets;
        Zones = zones;
    }

    public GcloudEdgeCloudNetworkingInterconnects Interconnects { get; }

    public GcloudEdgeCloudNetworkingNetworks Networks { get; }

    public GcloudEdgeCloudNetworkingOperations Operations { get; }

    public GcloudEdgeCloudNetworkingRouters Routers { get; }

    public GcloudEdgeCloudNetworkingSubnets Subnets { get; }

    public GcloudEdgeCloudNetworkingZones Zones { get; }
}