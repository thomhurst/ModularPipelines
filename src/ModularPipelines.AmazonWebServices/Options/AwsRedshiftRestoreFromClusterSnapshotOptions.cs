using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "restore-from-cluster-snapshot")]
public record AwsRedshiftRestoreFromClusterSnapshotOptions(
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--snapshot-identifier")]
    public string? SnapshotIdentifier { get; set; }

    [CommandSwitch("--snapshot-arn")]
    public string? SnapshotArn { get; set; }

    [CommandSwitch("--snapshot-cluster-identifier")]
    public string? SnapshotClusterIdentifier { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--cluster-subnet-group-name")]
    public string? ClusterSubnetGroupName { get; set; }

    [CommandSwitch("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CommandSwitch("--hsm-client-certificate-identifier")]
    public string? HsmClientCertificateIdentifier { get; set; }

    [CommandSwitch("--hsm-configuration-identifier")]
    public string? HsmConfigurationIdentifier { get; set; }

    [CommandSwitch("--elastic-ip")]
    public string? ElasticIp { get; set; }

    [CommandSwitch("--cluster-parameter-group-name")]
    public string? ClusterParameterGroupName { get; set; }

    [CommandSwitch("--cluster-security-groups")]
    public string[]? ClusterSecurityGroups { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--automated-snapshot-retention-period")]
    public int? AutomatedSnapshotRetentionPeriod { get; set; }

    [CommandSwitch("--manual-snapshot-retention-period")]
    public int? ManualSnapshotRetentionPeriod { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--node-type")]
    public string? NodeType { get; set; }

    [CommandSwitch("--additional-info")]
    public string? AdditionalInfo { get; set; }

    [CommandSwitch("--iam-roles")]
    public string[]? IamRoles { get; set; }

    [CommandSwitch("--maintenance-track-name")]
    public string? MaintenanceTrackName { get; set; }

    [CommandSwitch("--snapshot-schedule-identifier")]
    public string? SnapshotScheduleIdentifier { get; set; }

    [CommandSwitch("--number-of-nodes")]
    public int? NumberOfNodes { get; set; }

    [CommandSwitch("--aqua-configuration-status")]
    public string? AquaConfigurationStatus { get; set; }

    [CommandSwitch("--default-iam-role-arn")]
    public string? DefaultIamRoleArn { get; set; }

    [CommandSwitch("--reserved-node-id")]
    public string? ReservedNodeId { get; set; }

    [CommandSwitch("--target-reserved-node-offering-id")]
    public string? TargetReservedNodeOfferingId { get; set; }

    [CommandSwitch("--master-password-secret-kms-key-id")]
    public string? MasterPasswordSecretKmsKeyId { get; set; }

    [CommandSwitch("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}