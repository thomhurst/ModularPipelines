using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudEndpoints
{
    public GcloudEndpoints(
        GcloudEndpointsConfigs configs,
        GcloudEndpointsOperations operations,
        GcloudEndpointsServices services
    )
    {
        Configs = configs;
        Operations = operations;
        Services = services;
    }

    public GcloudEndpointsConfigs Configs { get; }

    public GcloudEndpointsOperations Operations { get; }

    public GcloudEndpointsServices Services { get; }
}