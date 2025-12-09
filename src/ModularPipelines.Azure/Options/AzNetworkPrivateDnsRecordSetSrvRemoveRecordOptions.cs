using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-dns", "record-set", "srv", "remove-record")]
public record AzNetworkPrivateDnsRecordSetSrvRemoveRecordOptions(
[property: CliOption("--port")] int Port,
[property: CliOption("--priority")] string Priority,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--target")] string Target,
[property: CliOption("--weight")] string Weight,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliFlag("--keep-empty-record-set")]
    public bool? KeepEmptyRecordSet { get; set; }
}