using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-dns", "record-set", "ptr", "remove-record")]
public record AzNetworkPrivateDnsRecordSetPtrRemoveRecordOptions(
[property: CliOption("--ptrdname")] string Ptrdname,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliFlag("--keep-empty-record-set")]
    public bool? KeepEmptyRecordSet { get; set; }
}