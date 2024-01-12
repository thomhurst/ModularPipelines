using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "aws", "clusters", "create")]
public record GcloudContainerAwsClustersCreateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--aws-region")] string AwsRegion,
[property: CommandSwitch("--cluster-version")] string ClusterVersion,
[property: CommandSwitch("--config-encryption-kms-key-arn")] string ConfigEncryptionKmsKeyArn,
[property: CommandSwitch("--database-encryption-kms-key-arn")] string DatabaseEncryptionKmsKeyArn,
[property: CommandSwitch("--fleet-project")] string FleetProject,
[property: CommandSwitch("--iam-instance-profile")] string IamInstanceProfile,
[property: CommandSwitch("--pod-address-cidr-blocks")] string PodAddressCidrBlocks,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--service-address-cidr-blocks")] string ServiceAddressCidrBlocks,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds,
[property: CommandSwitch("--vpc-id")] string VpcId
) : GcloudOptions
{
    [CommandSwitch("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [CommandSwitch("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [CommandSwitch("--annotations")]
    public string[]? Annotations { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disable-per-node-pool-sg-rules")]
    public bool? DisablePerNodePoolSgRules { get; set; }

    [BooleanCommandSwitch("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--logging")]
    public string[]? Logging { get; set; }

    [CommandSwitch("--main-volume-iops")]
    public string? MainVolumeIops { get; set; }

    [CommandSwitch("--main-volume-kms-key-arn")]
    public string? MainVolumeKmsKeyArn { get; set; }

    [CommandSwitch("--main-volume-size")]
    public string? MainVolumeSize { get; set; }

    [CommandSwitch("--main-volume-throughput")]
    public string? MainVolumeThroughput { get; set; }

    [CommandSwitch("--main-volume-type")]
    public string? MainVolumeType { get; set; }

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

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--ssh-ec2-key-pair")]
    public string? SshEc2KeyPair { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--proxy-secret-arn")]
    public string? ProxySecretArn { get; set; }

    [CommandSwitch("--proxy-secret-version-id")]
    public string? ProxySecretVersionId { get; set; }
}