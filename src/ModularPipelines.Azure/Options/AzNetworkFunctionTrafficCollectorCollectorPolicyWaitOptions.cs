using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-function", "traffic-collector", "collector-policy", "wait")]
public record AzNetworkFunctionTrafficCollectorCollectorPolicyWaitOptions : AzOptions
{
    [CliOption("--collector-policy-name")]
    public string? CollectorPolicyName { get; set; }

    [CliFlag("--created")]
    public bool? Created { get; set; }

    [CliOption("--custom")]
    public string? Custom { get; set; }

    [CliFlag("--deleted")]
    public bool? Deleted { get; set; }

    [CliFlag("--exists")]
    public bool? Exists { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--traffic-collector-name")]
    public string? TrafficCollectorName { get; set; }

    [CliFlag("--updated")]
    public bool? Updated { get; set; }
}