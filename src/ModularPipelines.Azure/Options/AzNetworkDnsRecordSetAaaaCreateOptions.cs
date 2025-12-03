using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "dns", "record-set", "aaaa", "create")]
public record AzNetworkDnsRecordSetAaaaCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--target-resource")]
    public string? TargetResource { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}