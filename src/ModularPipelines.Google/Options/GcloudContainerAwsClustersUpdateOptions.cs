using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "aws", "clusters", "update")]
public record GcloudContainerAwsClustersUpdateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [CommandSwitch("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CommandSwitch("--cluster-version")]
    public string? ClusterVersion { get; set; }

    [CommandSwitch("--config-encryption-kms-key-arn")]
    public string? ConfigEncryptionKmsKeyArn { get; set; }

    [CommandSwitch("--iam-instance-profile")]
    public string? IamInstanceProfile { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--logging")]
    public string[]? Logging { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--role-session-name")]
    public string? RoleSessionName { get; set; }

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

    [BooleanCommandSwitch("--clear-description")]
    public bool? ClearDescription { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

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

    [BooleanCommandSwitch("--disable-managed-prometheus")]
    public bool? DisableManagedPrometheus { get; set; }

    [BooleanCommandSwitch("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [BooleanCommandSwitch("--disable-per-node-pool-sg-rules")]
    public bool? DisablePerNodePoolSgRules { get; set; }

    [BooleanCommandSwitch("--enable-per-node-pool-sg-rules")]
    public bool? EnablePerNodePoolSgRules { get; set; }
}