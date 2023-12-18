using System.Diagnostics.CodeAnalysis;

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