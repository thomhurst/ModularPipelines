using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "create-target-group")]
public record AwsElbv2CreateTargetGroupOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--protocol-version")]
    public string? ProtocolVersion { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--vpc-id")]
    public string? VpcId { get; set; }

    [CliOption("--health-check-protocol")]
    public string? HealthCheckProtocol { get; set; }

    [CliOption("--health-check-port")]
    public string? HealthCheckPort { get; set; }

    [CliOption("--health-check-path")]
    public string? HealthCheckPath { get; set; }

    [CliOption("--health-check-interval-seconds")]
    public int? HealthCheckIntervalSeconds { get; set; }

    [CliOption("--health-check-timeout-seconds")]
    public int? HealthCheckTimeoutSeconds { get; set; }

    [CliOption("--healthy-threshold-count")]
    public int? HealthyThresholdCount { get; set; }

    [CliOption("--unhealthy-threshold-count")]
    public int? UnhealthyThresholdCount { get; set; }

    [CliOption("--matcher")]
    public string? Matcher { get; set; }

    [CliOption("--target-type")]
    public string? TargetType { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}