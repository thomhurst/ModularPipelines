using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "update-endpoint-group")]
public record AwsGlobalacceleratorUpdateEndpointGroupOptions(
[property: CommandSwitch("--endpoint-group-arn")] string EndpointGroupArn
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

    [CommandSwitch("--port-overrides")]
    public string[]? PortOverrides { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}