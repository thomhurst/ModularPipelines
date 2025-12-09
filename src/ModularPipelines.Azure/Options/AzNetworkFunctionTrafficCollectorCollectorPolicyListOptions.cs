using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network-function", "traffic-collector", "collector-policy", "list")]
public record AzNetworkFunctionTrafficCollectorCollectorPolicyListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--traffic-collector-name")] string TrafficCollectorName
) : AzOptions;