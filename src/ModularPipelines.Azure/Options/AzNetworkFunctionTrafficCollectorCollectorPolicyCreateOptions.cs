using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-function", "traffic-collector", "collector-policy", "create")]
public record AzNetworkFunctionTrafficCollectorCollectorPolicyCreateOptions(
[property: CliOption("--collector-policy-name")] string CollectorPolicyName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--traffic-collector-name")] string TrafficCollectorName
) : AzOptions
{
    [CliOption("--emission-policies")]
    public string? EmissionPolicies { get; set; }

    [CliOption("--ingestion-policy")]
    public string? IngestionPolicy { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}