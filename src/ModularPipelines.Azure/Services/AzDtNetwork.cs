using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt")]
public class AzDtNetwork
{
    public AzDtNetwork(
        AzDtNetworkPrivateEndpoint privateEndpoint,
        AzDtNetworkPrivateLink privateLink
    )
    {
        PrivateEndpoint = privateEndpoint;
        PrivateLink = privateLink;
    }

    public AzDtNetworkPrivateEndpoint PrivateEndpoint { get; }

    public AzDtNetworkPrivateLink PrivateLink { get; }
}