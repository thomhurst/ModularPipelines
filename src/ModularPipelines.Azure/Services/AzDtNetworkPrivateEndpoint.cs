using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "network")]
public class AzDtNetworkPrivateEndpoint
{
    public AzDtNetworkPrivateEndpoint(
        AzDtNetworkPrivateEndpointConnection connection
    )
    {
        Connection = connection;
    }

    public AzDtNetworkPrivateEndpointConnection Connection { get; }
}