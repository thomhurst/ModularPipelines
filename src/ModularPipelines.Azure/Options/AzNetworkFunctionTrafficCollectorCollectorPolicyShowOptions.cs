using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-function", "traffic-collector", "collector-policy", "show")]
public record AzNetworkFunctionTrafficCollectorCollectorPolicyShowOptions : AzOptions
{
    [CommandSwitch("--collector-policy-name")]
    public string? CollectorPolicyName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--traffic-collector-name")]
    public string? TrafficCollectorName { get; set; }
}