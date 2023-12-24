using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "create-endpoint-group")]
public record AwsGlobalacceleratorCreateEndpointGroupOptions(
[property: CommandSwitch("--listener-arn")] string ListenerArn,
[property: CommandSwitch("--endpoint-group-region")] string EndpointGroupRegion
) : AwsOptions
{
    [CommandSwitch("--endpoint-configurations")]
    public string[]? EndpointConfigurations { get; set; }

    [CommandSwitch("--traffic-dial-percentage")]
    public float? TrafficDialPercentage { get; set; }

    [CommandSwitch("--health-check-port")]
    public int? HealthCheckPort { get; set; }

    [CommandSwitch("--health-check-protocol")]
    public string? HealthCheckProtocol { get; set; }

    [CommandSwitch("--health-check-path")]
    public string? HealthCheckPath { get; set; }

    [CommandSwitch("--health-check-interval-seconds")]
    public int? HealthCheckIntervalSeconds { get; set; }

    [CommandSwitch("--threshold-count")]
    public int? ThresholdCount { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--port-overrides")]
    public string[]? PortOverrides { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}