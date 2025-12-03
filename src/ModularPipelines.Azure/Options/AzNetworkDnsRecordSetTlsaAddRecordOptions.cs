using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "dns", "record-set", "tlsa", "add-record")]
public record AzNetworkDnsRecordSetTlsaAddRecordOptions(
[property: CliOption("--certificate-data")] string CertificateData,
[property: CliOption("--certificate-usage")] string CertificateUsage,
[property: CliOption("--matching-type")] string MatchingType,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--selector")] string Selector,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}