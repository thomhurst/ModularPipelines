using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "dns", "record-set", "caa", "add-record")]
public record AzNetworkDnsRecordSetCaaAddRecordOptions(
[property: CliOption("--flags")] string Flags,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--tag")] string Tag,
[property: CliOption("--value")] string Value,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}