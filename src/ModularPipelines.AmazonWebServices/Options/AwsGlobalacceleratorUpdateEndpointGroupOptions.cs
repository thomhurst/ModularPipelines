using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "update-endpoint-group")]
public record AwsGlobalacceleratorUpdateEndpointGroupOptions(
[property: CliOption("--endpoint-group-arn")] string EndpointGroupArn
) : AwsOptions
{
    [CliOption("--endpoint-configurations")]
    public string[]? EndpointConfigurations { get; set; }

    [CliOption("--traffic-dial-percentage")]
    public float? TrafficDialPercentage { get; set; }

    [CliOption("--health-check-port")]
    public int? HealthCheckPort { get; set; }

    [CliOption("--health-check-protocol")]
    public string? HealthCheckProtocol { get; set; }

    [CliOption("--health-check-path")]
    public string? HealthCheckPath { get; set; }

    [CliOption("--health-check-interval-seconds")]
    public int? HealthCheckIntervalSeconds { get; set; }

    [CliOption("--threshold-count")]
    public int? ThresholdCount { get; set; }

    [CliOption("--port-overrides")]
    public string[]? PortOverrides { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}