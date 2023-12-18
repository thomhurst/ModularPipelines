using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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