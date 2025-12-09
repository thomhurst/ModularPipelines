using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "backend-pool", "list")]
public record AzNetworkFrontDoorBackendPoolListOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;