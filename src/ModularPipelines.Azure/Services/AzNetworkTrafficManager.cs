using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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