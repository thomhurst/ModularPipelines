using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "node-pools", "create")]
public record GcloudContainerAwsNodePoolsCreateOptions(
[property: CliArgument] string NodePool,
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliOption("--config-encryption-kms-key-arn")] string ConfigEncryptionKmsKeyArn,
[property: CliOption("--iam-instance-profile")] string IamInstanceProfile,
[property: CliOption("--max-pods-per-node")] string MaxPodsPerNode,
[property: CliOption("--node-version")] string NodeVersion,
[property: CliOption("--subnet-id")] string SubnetId,
[property: CliOption("--max-nodes")] string MaxNodes,
[property: CliOption("--min-nodes")] string MinNodes
) : GcloudOptions
{
    [CliOption("--annotations")]
    public string[]? Annotations { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--enable-autorepair")]
    public bool? EnableAutorepair { get; set; }

    [CliOption("--max-surge-update")]
    public string? MaxSurgeUpdate { get; set; }

    [CliOption("--max-unavailable-update")]
    public string? MaxUnavailableUpdate { get; set; }

    [CliOption("--node-labels")]
    public string[]? NodeLabels { get; set; }

    [CliOption("--node-taints")]
    public string[]? NodeTaints { get; set; }

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

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--ssh-ec2-key-pair")]
    public string? SshEc2KeyPair { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--autoscaling-metrics-granularity")]
    public string? AutoscalingMetricsGranularity { get; set; }

    [CliOption("--autoscaling-metrics")]
    public string[]? AutoscalingMetrics { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--spot-instance-types")]
    public string[]? SpotInstanceTypes { get; set; }

    [CliOption("--proxy-secret-arn")]
    public string? ProxySecretArn { get; set; }

    [CliOption("--proxy-secret-version-id")]
    public string? ProxySecretVersionId { get; set; }
}