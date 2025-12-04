using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "backend-pool", "backend", "remove")]
public record AzNetworkFrontDoorBackendPoolBackendRemoveOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--index")] string Index,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;