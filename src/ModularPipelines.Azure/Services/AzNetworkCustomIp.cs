using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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