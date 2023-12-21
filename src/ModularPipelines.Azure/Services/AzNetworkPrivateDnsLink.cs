using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "private-dns")]
public class AzNetworkPrivateDnsLink
{
    public AzNetworkPrivateDnsLink(
        AzNetworkPrivateDnsLinkVnet vnet
    )
    {
        Vnet = vnet;
    }

    public AzNetworkPrivateDnsLinkVnet Vnet { get; }
}