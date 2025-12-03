using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "backend-pool", "backend", "list")]
public record AzNetworkFrontDoorBackendPoolBackendListOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;