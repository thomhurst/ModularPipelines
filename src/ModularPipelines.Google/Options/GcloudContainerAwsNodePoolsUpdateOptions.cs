using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "node-pools", "update")]
public record GcloudContainerAwsNodePoolsUpdateOptions(
[property: CliArgument] string NodePool,
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--config-encryption-kms-key-arn")]
    public string? ConfigEncryptionKmsKeyArn { get; set; }

    [CliFlag("--enable-autorepair")]
    public bool? EnableAutorepair { get; set; }

    [CliOption("--iam-instance-profile")]
    public string? IamInstanceProfile { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--max-surge-update")]
    public string? MaxSurgeUpdate { get; set; }

    [CliOption("--max-unavailable-update")]
    public string? MaxUnavailableUpdate { get; set; }

    [CliOption("--node-version")]
    public string? NodeVersion { get; set; }

    [CliOption("--root-volume-iops")]
    public string? RootVolumeIops { get; set; }

    [CliOption("--root-volume-kms-key-arn")]
    public string? RootVolumeKmsKeyArn { get; set; }

    [CliOption("--root-volume-size")]
    public string? RootVolumeSize { get; set; }

    [CliOption("--root-volume-throughput")]
    public string? RootVolumeThroughput { get; set; }

    [CliOption("--root-volume-type")]
    public string? RootVolumeType { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--annotations")]
    public string[]? Annotations { get; set; }

    [CliFlag("--clear-annotations")]
    public bool? ClearAnnotations { get; set; }

    [CliFlag("--clear-autoscaling-metrics")]
    public bool? ClearAutoscalingMetrics { get; set; }

    [CliOption("--autoscaling-metrics")]
    public string[]? AutoscalingMetrics { get; set; }

    [CliOption("--autoscaling-metrics-granularity")]
    public string? AutoscalingMetricsGranularity { get; set; }

    [CliFlag("--clear-node-labels")]
    public bool? ClearNodeLabels { get; set; }

    [CliOption("--node-labels")]
    public string[]? NodeLabels { get; set; }

    [CliFlag("--clear-proxy-config")]
    public bool? ClearProxyConfig { get; set; }

    [CliOption("--proxy-secret-arn")]
    public string? ProxySecretArn { get; set; }

    [CliOption("--proxy-secret-version-id")]
    public string? ProxySecretVersionId { get; set; }

    [CliFlag("--clear-security-group-ids")]
    public bool? ClearSecurityGroupIds { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliFlag("--clear-ssh-ec2-key-pair")]
    public bool? ClearSshEc2KeyPair { get; set; }

    [CliOption("--ssh-ec2-key-pair")]
    public string? SshEc2KeyPair { get; set; }

    [CliFlag("--clear-tags")]
    public bool? ClearTags { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CliOption("--min-nodes")]
    public string? MinNodes { get; set; }
}