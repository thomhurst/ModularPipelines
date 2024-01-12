using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudMemcache
{
    public GcloudMemcache(
        GcloudMemcacheInstances instances,
        GcloudMemcacheOperations operations,
        GcloudMemcacheRegions regions
    )
    {
        Instances = instances;
        Operations = operations;
        Regions = regions;
    }

    public GcloudMemcacheInstances Instances { get; }

    public GcloudMemcacheOperations Operations { get; }

    public GcloudMemcacheRegions Regions { get; }
}