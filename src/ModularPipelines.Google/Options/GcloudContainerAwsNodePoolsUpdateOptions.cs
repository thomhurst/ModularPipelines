using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "aws", "node-pools", "update")]
public record GcloudContainerAwsNodePoolsUpdateOptions(
[property: PositionalArgument] string NodePool,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--config-encryption-kms-key-arn")]
    public string? ConfigEncryptionKmsKeyArn { get; set; }

    [BooleanCommandSwitch("--enable-autorepair")]
    public bool? EnableAutorepair { get; set; }

    [CommandSwitch("--iam-instance-profile")]
    public string? IamInstanceProfile { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--max-surge-update")]
    public string? MaxSurgeUpdate { get; set; }

    [CommandSwitch("--max-unavailable-update")]
    public string? MaxUnavailableUpdate { get; set; }

    [CommandSwitch("--node-version")]
    public string? NodeVersion { get; set; }

    [CommandSwitch("--root-volume-iops")]
    public string? RootVolumeIops { get; set; }

    [CommandSwitch("--root-volume-kms-key-arn")]
    public string? RootVolumeKmsKeyArn { get; set; }

    [CommandSwitch("--root-volume-size")]
    public string? RootVolumeSize { get; set; }

    [CommandSwitch("--root-volume-throughput")]
    public string? RootVolumeThroughput { get; set; }

    [CommandSwitch("--root-volume-type")]
    public string? RootVolumeType { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--annotations")]
    public string[]? Annotations { get; set; }

    [BooleanCommandSwitch("--clear-annotations")]
    public bool? ClearAnnotations { get; set; }

    [BooleanCommandSwitch("--clear-autoscaling-metrics")]
    public bool? ClearAutoscalingMetrics { get; set; }

    [CommandSwitch("--autoscaling-metrics")]
    public string[]? AutoscalingMetrics { get; set; }

    [CommandSwitch("--autoscaling-metrics-granularity")]
    public string? AutoscalingMetricsGranularity { get; set; }

    [BooleanCommandSwitch("--clear-node-labels")]
    public bool? ClearNodeLabels { get; set; }

    [CommandSwitch("--node-labels")]
    public string[]? NodeLabels { get; set; }

    [BooleanCommandSwitch("--clear-proxy-config")]
    public bool? ClearProxyConfig { get; set; }

    [CommandSwitch("--proxy-secret-arn")]
    public string? ProxySecretArn { get; set; }

    [CommandSwitch("--proxy-secret-version-id")]
    public string? ProxySecretVersionId { get; set; }

    [BooleanCommandSwitch("--clear-security-group-ids")]
    public bool? ClearSecurityGroupIds { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [BooleanCommandSwitch("--clear-ssh-ec2-key-pair")]
    public bool? ClearSshEc2KeyPair { get; set; }

    [CommandSwitch("--ssh-ec2-key-pair")]
    public string? SshEc2KeyPair { get; set; }

    [BooleanCommandSwitch("--clear-tags")]
    public bool? ClearTags { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CommandSwitch("--min-nodes")]
    public string? MinNodes { get; set; }
}