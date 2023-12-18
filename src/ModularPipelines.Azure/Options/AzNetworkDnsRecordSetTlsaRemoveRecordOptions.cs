using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "tlsa", "remove-record")]
public record AzNetworkDnsRecordSetTlsaRemoveRecordOptions(
[property: CommandSwitch("--certificate-data")] string CertificateData,
[property: CommandSwitch("--certificate-usage")] string CertificateUsage,
[property: CommandSwitch("--matching-type")] string MatchingType,
[property: CommandSwitch("--record-set-name")] string RecordSetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--selector")] string Selector,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [BooleanCommandSwitch("--keep-empty-record-set")]
    public bool? KeepEmptyRecordSet { get; set; }
}