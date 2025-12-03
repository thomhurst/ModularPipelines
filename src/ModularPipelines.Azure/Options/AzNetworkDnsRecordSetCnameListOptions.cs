using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "dns", "record-set", "cname", "list")]
public record AzNetworkDnsRecordSetCnameListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--recordsetnamesuffix")]
    public string? Recordsetnamesuffix { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}