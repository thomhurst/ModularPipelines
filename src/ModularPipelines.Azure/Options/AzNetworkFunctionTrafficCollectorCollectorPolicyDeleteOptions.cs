using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-function", "traffic-collector", "collector-policy", "delete")]
public record AzNetworkFunctionTrafficCollectorCollectorPolicyDeleteOptions : AzOptions
{
    [CliOption("--collector-policy-name")]
    public string? CollectorPolicyName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--traffic-collector-name")]
    public string? TrafficCollectorName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}