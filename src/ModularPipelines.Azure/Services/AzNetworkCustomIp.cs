using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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