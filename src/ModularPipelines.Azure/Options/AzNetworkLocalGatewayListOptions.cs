using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "local-gateway", "list")]
public record AzNetworkLocalGatewayListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;