using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "dns", "record-set", "ds", "remove-record")]
public record AzNetworkDnsRecordSetDsRemoveRecordOptions(
[property: CliOption("--algorithm")] string Algorithm,
[property: CliOption("--digest")] string Digest,
[property: CliOption("--digest-type")] string DigestType,
[property: CliOption("--key-tag")] string KeyTag,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliFlag("--keep-empty-record-set")]
    public bool? KeepEmptyRecordSet { get; set; }
}