using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudEdgeCloud
{
    public GcloudEdgeCloud(
        GcloudEdgeCloudContainer container,
        GcloudEdgeCloudNetworking networking
    )
    {
        Container = container;
        Networking = networking;
    }

    public GcloudEdgeCloudContainer Container { get; }

    public GcloudEdgeCloudNetworking Networking { get; }
}