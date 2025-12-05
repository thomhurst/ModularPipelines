using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "restore-from-cluster-snapshot")]
public record AwsRedshiftRestoreFromClusterSnapshotOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CliOption("--snapshot-identifier")]
    public string? SnapshotIdentifier { get; set; }

    [CliOption("--snapshot-arn")]
    public string? SnapshotArn { get; set; }

    [CliOption("--snapshot-cluster-identifier")]
    public string? SnapshotClusterIdentifier { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--cluster-subnet-group-name")]
    public string? ClusterSubnetGroupName { get; set; }

    [CliOption("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CliOption("--hsm-client-certificate-identifier")]
    public string? HsmClientCertificateIdentifier { get; set; }

    [CliOption("--hsm-configuration-identifier")]
    public string? HsmConfigurationIdentifier { get; set; }

    [CliOption("--elastic-ip")]
    public string? ElasticIp { get; set; }

    [CliOption("--cluster-parameter-group-name")]
    public string? ClusterParameterGroupName { get; set; }

    [CliOption("--cluster-security-groups")]
    public string[]? ClusterSecurityGroups { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--automated-snapshot-retention-period")]
    public int? AutomatedSnapshotRetentionPeriod { get; set; }

    [CliOption("--manual-snapshot-retention-period")]
    public int? ManualSnapshotRetentionPeriod { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--node-type")]
    public string? NodeType { get; set; }

    [CliOption("--additional-info")]
    public string? AdditionalInfo { get; set; }

    [CliOption("--iam-roles")]
    public string[]? IamRoles { get; set; }

    [CliOption("--maintenance-track-name")]
    public string? MaintenanceTrackName { get; set; }

    [CliOption("--snapshot-schedule-identifier")]
    public string? SnapshotScheduleIdentifier { get; set; }

    [CliOption("--number-of-nodes")]
    public int? NumberOfNodes { get; set; }

    [CliOption("--aqua-configuration-status")]
    public string? AquaConfigurationStatus { get; set; }

    [CliOption("--default-iam-role-arn")]
    public string? DefaultIamRoleArn { get; set; }

    [CliOption("--reserved-node-id")]
    public string? ReservedNodeId { get; set; }

    [CliOption("--target-reserved-node-offering-id")]
    public string? TargetReservedNodeOfferingId { get; set; }

    [CliOption("--master-password-secret-kms-key-id")]
    public string? MasterPasswordSecretKmsKeyId { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}