using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudLooker
{
    public GcloudLooker(
        GcloudLookerInstances instances,
        GcloudLookerOperations operations,
        GcloudLookerRegions regions
    )
    {
        Instances = instances;
        Operations = operations;
        Regions = regions;
    }

    public GcloudLookerInstances Instances { get; }

    public GcloudLookerOperations Operations { get; }

    public GcloudLookerRegions Regions { get; }
}