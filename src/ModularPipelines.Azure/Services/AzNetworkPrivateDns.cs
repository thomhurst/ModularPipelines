using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network")]
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