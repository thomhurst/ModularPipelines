using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-endpoint", "ip-config", "list")]
public record AzNetworkPrivateEndpointIpConfigListOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;