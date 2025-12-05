using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "clusters", "create")]
public record GcloudContainerAwsClustersCreateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliOption("--aws-region")] string AwsRegion,
[property: CliOption("--cluster-version")] string ClusterVersion,
[property: CliOption("--config-encryption-kms-key-arn")] string ConfigEncryptionKmsKeyArn,
[property: CliOption("--database-encryption-kms-key-arn")] string DatabaseEncryptionKmsKeyArn,
[property: CliOption("--fleet-project")] string FleetProject,
[property: CliOption("--iam-instance-profile")] string IamInstanceProfile,
[property: CliOption("--pod-address-cidr-blocks")] string PodAddressCidrBlocks,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--service-address-cidr-blocks")] string ServiceAddressCidrBlocks,
[property: CliOption("--subnet-ids")] string[] SubnetIds,
[property: CliOption("--vpc-id")] string VpcId
) : GcloudOptions
{
    [CliOption("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [CliOption("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [CliOption("--annotations")]
    public string[]? Annotations { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disable-per-node-pool-sg-rules")]
    public bool? DisablePerNodePoolSgRules { get; set; }

    [CliFlag("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--logging")]
    public string[]? Logging { get; set; }

    [CliOption("--main-volume-iops")]
    public string? MainVolumeIops { get; set; }

    [CliOption("--main-volume-kms-key-arn")]
    public string? MainVolumeKmsKeyArn { get; set; }

    [CliOption("--main-volume-size")]
    public string? MainVolumeSize { get; set; }

    [CliOption("--main-volume-throughput")]
    public string? MainVolumeThroughput { get; set; }

    [CliOption("--main-volume-type")]
    public string? MainVolumeType { get; set; }

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

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--ssh-ec2-key-pair")]
    public string? SshEc2KeyPair { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--proxy-secret-arn")]
    public string? ProxySecretArn { get; set; }

    [CliOption("--proxy-secret-version-id")]
    public string? ProxySecretVersionId { get; set; }
}