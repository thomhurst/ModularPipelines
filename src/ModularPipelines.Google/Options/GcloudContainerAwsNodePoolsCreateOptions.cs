using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "aws", "node-pools", "create")]
public record GcloudContainerAwsNodePoolsCreateOptions(
[property: PositionalArgument] string NodePool,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--config-encryption-kms-key-arn")] string ConfigEncryptionKmsKeyArn,
[property: CommandSwitch("--iam-instance-profile")] string IamInstanceProfile,
[property: CommandSwitch("--max-pods-per-node")] string MaxPodsPerNode,
[property: CommandSwitch("--node-version")] string NodeVersion,
[property: CommandSwitch("--subnet-id")] string SubnetId,
[property: CommandSwitch("--max-nodes")] string MaxNodes,
[property: CommandSwitch("--min-nodes")] string MinNodes
) : GcloudOptions
{
    [CommandSwitch("--annotations")]
    public string[]? Annotations { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--enable-autorepair")]
    public bool? EnableAutorepair { get; set; }

    [CommandSwitch("--max-surge-update")]
    public string? MaxSurgeUpdate { get; set; }

    [CommandSwitch("--max-unavailable-update")]
    public string? MaxUnavailableUpdate { get; set; }

    [CommandSwitch("--node-labels")]
    public string[]? NodeLabels { get; set; }

    [CommandSwitch("--node-taints")]
    public string[]? NodeTaints { get; set; }

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

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--ssh-ec2-key-pair")]
    public string? SshEc2KeyPair { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--autoscaling-metrics-granularity")]
    public string? AutoscalingMetricsGranularity { get; set; }

    [CommandSwitch("--autoscaling-metrics")]
    public string[]? AutoscalingMetrics { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--spot-instance-types")]
    public string[]? SpotInstanceTypes { get; set; }

    [CommandSwitch("--proxy-secret-arn")]
    public string? ProxySecretArn { get; set; }

    [CommandSwitch("--proxy-secret-version-id")]
    public string? ProxySecretVersionId { get; set; }
}