using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "load-balancing", "list")]
public record AzNetworkFrontDoorLoadBalancingListOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;