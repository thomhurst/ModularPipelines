using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkPrivateDns
{
    public AzNetworkPrivateDns(
        AzNetworkPrivateDnsLink link,
        AzNetworkPrivateDnsRecordSet recordSet,
        AzNetworkPrivateDnsZone zone
    )
    {
        Link = link;
        RecordSet = recordSet;
        Zone = zone;
    }

    public AzNetworkPrivateDnsLink Link { get; }

    public AzNetworkPrivateDnsRecordSet RecordSet { get; }

    public AzNetworkPrivateDnsZone Zone { get; }
}