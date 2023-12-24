using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "modify-target-group")]
public record AwsElbv2ModifyTargetGroupOptions(
[property: CommandSwitch("--target-group-arn")] string TargetGroupArn
) : AwsOptions
{
    [CommandSwitch("--health-check-protocol")]
    public string? HealthCheckProtocol { get; set; }

    [CommandSwitch("--health-check-port")]
    public string? HealthCheckPort { get; set; }

    [CommandSwitch("--health-check-path")]
    public string? HealthCheckPath { get; set; }

    [CommandSwitch("--health-check-interval-seconds")]
    public int? HealthCheckIntervalSeconds { get; set; }

    [CommandSwitch("--health-check-timeout-seconds")]
    public int? HealthCheckTimeoutSeconds { get; set; }

    [CommandSwitch("--healthy-threshold-count")]
    public int? HealthyThresholdCount { get; set; }

    [CommandSwitch("--unhealthy-threshold-count")]
    public int? UnhealthyThresholdCount { get; set; }

    [CommandSwitch("--matcher")]
    public string? Matcher { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}