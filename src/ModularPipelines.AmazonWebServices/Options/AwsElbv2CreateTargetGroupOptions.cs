using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "create-target-group")]
public record AwsElbv2CreateTargetGroupOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--protocol-version")]
    public string? ProtocolVersion { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--vpc-id")]
    public string? VpcId { get; set; }

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

    [CommandSwitch("--target-type")]
    public string? TargetType { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}