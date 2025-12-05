using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "create-cluster")]
public record AwsEmrCreateClusterOptions(
[property: CliOption("--release-label")] string ReleaseLabel,
[property: CliOption("--ami-version")] string AmiVersion,
[property: CliOption("--instance-groups")] string[] InstanceGroups,
[property: CliOption("--instance-type")] string InstanceType,
[property: CliOption("--instance-count")] string InstanceCount,
[property: CliOption("--instance-fleets")] string[] InstanceFleets
) : AwsOptions
{
    [CliOption("--os-release-label")]
    public string? OsReleaseLabel { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--log-uri")]
    public string? LogUri { get; set; }

    [CliOption("--log-encryption-kms-key-id")]
    public string? LogEncryptionKmsKeyId { get; set; }

    [CliOption("--service-role")]
    public string? ServiceRole { get; set; }

    [CliOption("--auto-scaling-role")]
    public string? AutoScalingRole { get; set; }

    [CliFlag("--use-default-roles")]
    public bool? UseDefaultRoles { get; set; }

    [CliOption("--configurations")]
    public string? Configurations { get; set; }

    [CliOption("--ec2-attributes")]
    public string? Ec2Attributes { get; set; }

    [CliOption("--scale-down-behavior")]
    public string? ScaleDownBehavior { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--bootstrap-actions")]
    public string[]? BootstrapActions { get; set; }

    [CliOption("--applications")]
    public string[]? Applications { get; set; }

    [CliOption("--emrfs")]
    public string? Emrfs { get; set; }

    [CliOption("--steps")]
    public string[]? Steps { get; set; }

    [CliOption("--additional-info")]
    public string? AdditionalInfo { get; set; }

    [CliOption("--restore-from-hbase-backup")]
    public string? RestoreFromHbaseBackup { get; set; }

    [CliOption("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CliOption("--custom-ami-id")]
    public string? CustomAmiId { get; set; }

    [CliOption("--ebs-root-volume-size")]
    public string? EbsRootVolumeSize { get; set; }

    [CliOption("--ebs-root-volume-iops")]
    public string? EbsRootVolumeIops { get; set; }

    [CliOption("--ebs-root-volume-throughput")]
    public string? EbsRootVolumeThroughput { get; set; }

    [CliOption("--repo-upgrade-on-boot")]
    public string? RepoUpgradeOnBoot { get; set; }

    [CliOption("--kerberos-attributes")]
    public string? KerberosAttributes { get; set; }

    [CliOption("--step-concurrency-level")]
    public int? StepConcurrencyLevel { get; set; }

    [CliOption("--managed-scaling-policy")]
    public string? ManagedScalingPolicy { get; set; }

    [CliOption("--placement-group-configs")]
    public string[]? PlacementGroupConfigs { get; set; }

    [CliOption("--auto-termination-policy")]
    public string? AutoTerminationPolicy { get; set; }
}