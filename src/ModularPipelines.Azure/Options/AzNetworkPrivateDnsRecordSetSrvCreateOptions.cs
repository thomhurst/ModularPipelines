using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-dns", "record-set", "srv", "create")]
public record AzNetworkPrivateDnsRecordSetSrvCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}