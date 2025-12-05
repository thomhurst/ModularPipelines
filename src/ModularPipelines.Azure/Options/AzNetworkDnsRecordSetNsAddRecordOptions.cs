using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "dns", "record-set", "ns", "add-record")]
public record AzNetworkDnsRecordSetNsAddRecordOptions(
[property: CliOption("--nsdname")] string Nsdname,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--subscriptionid")]
    public string? Subscriptionid { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}