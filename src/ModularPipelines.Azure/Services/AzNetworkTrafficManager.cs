using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkTrafficManager
{
    public AzNetworkTrafficManager(
        AzNetworkTrafficManagerEndpoint endpoint,
        AzNetworkTrafficManagerProfile profile
    )
    {
        Endpoint = endpoint;
        Profile = profile;
    }

    public AzNetworkTrafficManagerEndpoint Endpoint { get; }

    public AzNetworkTrafficManagerProfile Profile { get; }
}