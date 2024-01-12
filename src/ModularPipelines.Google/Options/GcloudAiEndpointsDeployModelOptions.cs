using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "endpoints", "deploy-model")]
public record GcloudAiEndpointsDeployModelOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--model")] string Model
) : GcloudOptions
{
    [CommandSwitch("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CommandSwitch("--autoscaling-metric-specs")]
    public string[]? AutoscalingMetricSpecs { get; set; }

    [CommandSwitch("--deployed-model-id")]
    public string? DeployedModelId { get; set; }

    [BooleanCommandSwitch("--disable-container-logging")]
    public bool? DisableContainerLogging { get; set; }

    [BooleanCommandSwitch("--enable-access-logging")]
    public bool? EnableAccessLogging { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--max-replica-count")]
    public string? MaxReplicaCount { get; set; }

    [CommandSwitch("--min-replica-count")]
    public string? MinReplicaCount { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--traffic-split")]
    public string[]? TrafficSplit { get; set; }
}