using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkCustomIp
{
    public AzNetworkCustomIp(
        AzNetworkCustomIpPrefix prefix
    )
    {
        Prefix = prefix;
    }

    public AzNetworkCustomIpPrefix Prefix { get; }
}