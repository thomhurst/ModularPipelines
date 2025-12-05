using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "modify-target-group")]
public record AwsElbv2ModifyTargetGroupOptions(
[property: CliOption("--target-group-arn")] string TargetGroupArn
) : AwsOptions
{
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

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}