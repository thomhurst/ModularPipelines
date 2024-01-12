using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudIds
{
    public GcloudIds(
        GcloudIdsEndpoints endpoints
    )
    {
        Endpoints = endpoints;
    }

    public GcloudIdsEndpoints Endpoints { get; }
}