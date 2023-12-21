using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzNetworkFunction
{
    public AzNetworkFunction(
        AzNetworkFunctionTrafficCollector trafficCollector
    )
    {
        TrafficCollector = trafficCollector;
    }

    public AzNetworkFunctionTrafficCollector TrafficCollector { get; }
}