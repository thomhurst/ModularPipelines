using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network")]
public class AzNetworkNat
{
    public AzNetworkNat(
        AzNetworkNatGateway gateway
    )
    {
        Gateway = gateway;
    }

    public AzNetworkNatGateway Gateway { get; }
}