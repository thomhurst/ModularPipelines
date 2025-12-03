using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "private-endpoint", "dns-zone-group", "add")]
public record AzNetworkPrivateEndpointDnsZoneGroupAddOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--private-dns-zone")] string PrivateDnsZone,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}