using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "create-cluster")]
public record AwsEmrCreateClusterOptions(
[property: CommandSwitch("--release-label")] string ReleaseLabel,
[property: CommandSwitch("--ami-version")] string AmiVersion,
[property: CommandSwitch("--instance-groups")] string[] InstanceGroups,
[property: CommandSwitch("--instance-type")] string InstanceType,
[property: CommandSwitch("--instance-count")] string InstanceCount,
[property: CommandSwitch("--instance-fleets")] string[] InstanceFleets
) : AwsOptions
{
    [CommandSwitch("--os-release-label")]
    public string? OsReleaseLabel { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--log-uri")]
    public string? LogUri { get; set; }

    [CommandSwitch("--log-encryption-kms-key-id")]
    public string? LogEncryptionKmsKeyId { get; set; }

    [CommandSwitch("--service-role")]
    public string? ServiceRole { get; set; }

    [CommandSwitch("--auto-scaling-role")]
    public string? AutoScalingRole { get; set; }

    [BooleanCommandSwitch("--use-default-roles")]
    public bool? UseDefaultRoles { get; set; }

    [CommandSwitch("--configurations")]
    public string? Configurations { get; set; }

    [CommandSwitch("--ec2-attributes")]
    public string? Ec2Attributes { get; set; }

    [CommandSwitch("--scale-down-behavior")]
    public string? ScaleDownBehavior { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--bootstrap-actions")]
    public string[]? BootstrapActions { get; set; }

    [CommandSwitch("--applications")]
    public string[]? Applications { get; set; }

    [CommandSwitch("--emrfs")]
    public string? Emrfs { get; set; }

    [CommandSwitch("--steps")]
    public string[]? Steps { get; set; }

    [CommandSwitch("--additional-info")]
    public string? AdditionalInfo { get; set; }

    [CommandSwitch("--restore-from-hbase-backup")]
    public string? RestoreFromHbaseBackup { get; set; }

    [CommandSwitch("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CommandSwitch("--custom-ami-id")]
    public string? CustomAmiId { get; set; }

    [CommandSwitch("--ebs-root-volume-size")]
    public string? EbsRootVolumeSize { get; set; }

    [CommandSwitch("--ebs-root-volume-iops")]
    public string? EbsRootVolumeIops { get; set; }

    [CommandSwitch("--ebs-root-volume-throughput")]
    public string? EbsRootVolumeThroughput { get; set; }

    [CommandSwitch("--repo-upgrade-on-boot")]
    public string? RepoUpgradeOnBoot { get; set; }

    [CommandSwitch("--kerberos-attributes")]
    public string? KerberosAttributes { get; set; }

    [CommandSwitch("--step-concurrency-level")]
    public int? StepConcurrencyLevel { get; set; }

    [CommandSwitch("--managed-scaling-policy")]
    public string? ManagedScalingPolicy { get; set; }

    [CommandSwitch("--placement-group-configs")]
    public string[]? PlacementGroupConfigs { get; set; }

    [CommandSwitch("--auto-termination-policy")]
    public string? AutoTerminationPolicy { get; set; }
}