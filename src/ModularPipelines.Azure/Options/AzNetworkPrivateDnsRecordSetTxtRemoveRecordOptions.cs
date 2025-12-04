using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-dns", "record-set", "txt", "remove-record")]
public record AzNetworkPrivateDnsRecordSetTxtRemoveRecordOptions(
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--value")] string Value,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliFlag("--keep-empty-record-set")]
    public bool? KeepEmptyRecordSet { get; set; }
}