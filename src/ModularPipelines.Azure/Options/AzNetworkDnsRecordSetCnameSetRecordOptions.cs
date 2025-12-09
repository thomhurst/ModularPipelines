using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "dns", "record-set", "cname", "set-record")]
public record AzNetworkDnsRecordSetCnameSetRecordOptions(
[property: CliOption("--cname")] string Cname,
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