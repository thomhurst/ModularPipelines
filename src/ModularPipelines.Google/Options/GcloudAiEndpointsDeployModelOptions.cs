using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "endpoints", "deploy-model")]
public record GcloudAiEndpointsDeployModelOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Region,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--model")] string Model
) : GcloudOptions
{
    [CliOption("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CliOption("--autoscaling-metric-specs")]
    public string[]? AutoscalingMetricSpecs { get; set; }

    [CliOption("--deployed-model-id")]
    public string? DeployedModelId { get; set; }

    [CliFlag("--disable-container-logging")]
    public bool? DisableContainerLogging { get; set; }

    [CliFlag("--enable-access-logging")]
    public bool? EnableAccessLogging { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--max-replica-count")]
    public string? MaxReplicaCount { get; set; }

    [CliOption("--min-replica-count")]
    public string? MinReplicaCount { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--traffic-split")]
    public string[]? TrafficSplit { get; set; }
}