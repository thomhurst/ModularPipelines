using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "dns", "record-set", "mx", "remove-record")]
public record AzNetworkDnsRecordSetMxRemoveRecordOptions(
[property: CliOption("--exchange")] string Exchange,
[property: CliOption("--preference")] string Preference,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliFlag("--keep-empty-record-set")]
    public bool? KeepEmptyRecordSet { get; set; }
}