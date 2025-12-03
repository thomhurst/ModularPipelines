using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "dns", "record-set", "a", "list")]
public record AzNetworkDnsRecordSetAListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--recordsetnamesuffix")]
    public string? Recordsetnamesuffix { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}