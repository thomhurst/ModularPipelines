using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-function", "traffic-collector", "collector-policy", "list")]
public record AzNetworkFunctionTrafficCollectorCollectorPolicyListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--traffic-collector-name")] string TrafficCollectorName
) : AzOptions;