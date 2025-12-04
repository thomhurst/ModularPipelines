using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-endpoint", "dns-zone-group", "list")]
public record AzNetworkPrivateEndpointDnsZoneGroupListOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;