using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-function", "traffic-collector", "collector-policy", "show")]
public record AzNetworkFunctionTrafficCollectorCollectorPolicyShowOptions : AzOptions
{
    [CliOption("--collector-policy-name")]
    public string? CollectorPolicyName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--traffic-collector-name")]
    public string? TrafficCollectorName { get; set; }
}