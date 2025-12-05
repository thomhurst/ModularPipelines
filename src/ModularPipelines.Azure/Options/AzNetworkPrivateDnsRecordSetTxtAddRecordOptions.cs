using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-dns", "record-set", "txt", "add-record")]
public record AzNetworkPrivateDnsRecordSetTxtAddRecordOptions(
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--value")] string Value,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions;