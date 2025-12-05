using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "clusters", "update")]
public record GcloudContainerAwsClustersUpdateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [CliOption("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CliOption("--cluster-version")]
    public string? ClusterVersion { get; set; }

    [CliOption("--config-encryption-kms-key-arn")]
    public string? ConfigEncryptionKmsKeyArn { get; set; }

    [CliOption("--iam-instance-profile")]
    public string? IamInstanceProfile { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--logging")]
    public string[]? Logging { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--role-session-name")]
    public string? RoleSessionName { get; set; }

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

    [CliFlag("--clear-description")]
    public bool? ClearDescription { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

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

    [CliFlag("--disable-managed-prometheus")]
    public bool? DisableManagedPrometheus { get; set; }

    [CliFlag("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [CliFlag("--disable-per-node-pool-sg-rules")]
    public bool? DisablePerNodePoolSgRules { get; set; }

    [CliFlag("--enable-per-node-pool-sg-rules")]
    public bool? EnablePerNodePoolSgRules { get; set; }
}