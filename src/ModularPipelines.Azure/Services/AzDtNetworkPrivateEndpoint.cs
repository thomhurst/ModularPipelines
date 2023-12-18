using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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