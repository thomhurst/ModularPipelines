using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "private-dns", "record-set", "mx", "add-record")]
public record AzNetworkPrivateDnsRecordSetMxAddRecordOptions(
[property: CliOption("--exchange")] string Exchange,
[property: CliOption("--preference")] string Preference,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions;