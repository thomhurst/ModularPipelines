using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-function", "traffic-collector", "collector-policy", "create")]
public record AzNetworkFunctionTrafficCollectorCollectorPolicyCreateOptions(
[property: CommandSwitch("--collector-policy-name")] string CollectorPolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--traffic-collector-name")] string TrafficCollectorName
) : AzOptions
{
    [CommandSwitch("--emission-policies")]
    public string? EmissionPolicies { get; set; }

    [CommandSwitch("--ingestion-policy")]
    public string? IngestionPolicy { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}