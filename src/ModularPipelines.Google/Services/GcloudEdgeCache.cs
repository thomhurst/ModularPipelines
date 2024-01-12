using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudEdgeCache
{
    public GcloudEdgeCache(
        GcloudEdgeCacheKeysets keysets,
        GcloudEdgeCacheOperations operations,
        GcloudEdgeCacheOrigins origins,
        GcloudEdgeCacheServices services
    )
    {
        Keysets = keysets;
        Operations = operations;
        Origins = origins;
        Services = services;
    }

    public GcloudEdgeCacheKeysets Keysets { get; }

    public GcloudEdgeCacheOperations Operations { get; }

    public GcloudEdgeCacheOrigins Origins { get; }

    public GcloudEdgeCacheServices Services { get; }
}