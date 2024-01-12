using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudRedis
{
    public GcloudRedis(
        GcloudRedisClusters clusters,
        GcloudRedisInstances instances,
        GcloudRedisOperations operations,
        GcloudRedisRegions regions,
        GcloudRedisZones zones
    )
    {
        Clusters = clusters;
        Instances = instances;
        Operations = operations;
        Regions = regions;
        Zones = zones;
    }

    public GcloudRedisClusters Clusters { get; }

    public GcloudRedisInstances Instances { get; }

    public GcloudRedisOperations Operations { get; }

    public GcloudRedisRegions Regions { get; }

    public GcloudRedisZones Zones { get; }
}