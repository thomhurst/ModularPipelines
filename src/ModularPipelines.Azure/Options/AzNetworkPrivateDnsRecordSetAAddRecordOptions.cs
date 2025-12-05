using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-dns", "record-set", "a", "add-record")]
public record AzNetworkPrivateDnsRecordSetAAddRecordOptions(
[property: CliOption("--ipv4-address")] string Ipv4Address,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions;