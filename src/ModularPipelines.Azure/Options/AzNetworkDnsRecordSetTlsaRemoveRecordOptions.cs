using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "dns", "record-set", "tlsa", "remove-record")]
public record AzNetworkDnsRecordSetTlsaRemoveRecordOptions(
[property: CliOption("--certificate-data")] string CertificateData,
[property: CliOption("--certificate-usage")] string CertificateUsage,
[property: CliOption("--matching-type")] string MatchingType,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--selector")] string Selector,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliFlag("--keep-empty-record-set")]
    public bool? KeepEmptyRecordSet { get; set; }
}