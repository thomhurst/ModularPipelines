using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-endpoint", "dns-zone-group", "create")]
public record AzNetworkPrivateEndpointDnsZoneGroupCreateOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--name")] string Name,
[property: CliOption("--private-dns-zone")] string PrivateDnsZone,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}