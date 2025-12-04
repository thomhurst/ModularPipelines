using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "dns", "record-set", "ds", "add-record")]
public record AzNetworkDnsRecordSetDsAddRecordOptions(
[property: CliOption("--algorithm")] string Algorithm,
[property: CliOption("--digest")] string Digest,
[property: CliOption("--digest-type")] string DigestType,
[property: CliOption("--key-tag")] string KeyTag,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}