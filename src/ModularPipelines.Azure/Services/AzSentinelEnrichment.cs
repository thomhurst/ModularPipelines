using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel")]
public class AzSentinelEnrichment
{
    public AzSentinelEnrichment(
        AzSentinelEnrichmentDomainWhois domainWhois,
        AzSentinelEnrichmentIpGeodata ipGeodata
    )
    {
        DomainWhois = domainWhois;
        IpGeodata = ipGeodata;
    }

    public AzSentinelEnrichmentDomainWhois DomainWhois { get; }

    public AzSentinelEnrichmentIpGeodata IpGeodata { get; }
}