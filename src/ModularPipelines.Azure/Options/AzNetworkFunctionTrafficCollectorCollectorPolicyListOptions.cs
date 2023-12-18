using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-function", "traffic-collector", "collector-policy", "list")]
public record AzNetworkFunctionTrafficCollectorCollectorPolicyListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--traffic-collector-name")] string TrafficCollectorName
) : AzOptions
{
    [CommandSwitch("--collector-policy-name")]
    public string? CollectorPolicyName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

