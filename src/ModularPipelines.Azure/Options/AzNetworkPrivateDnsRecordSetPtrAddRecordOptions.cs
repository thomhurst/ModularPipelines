using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "private-dns", "record-set", "ptr", "add-record")]
public record AzNetworkPrivateDnsRecordSetPtrAddRecordOptions(
[property: CliOption("--ptrdname")] string Ptrdname,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions;