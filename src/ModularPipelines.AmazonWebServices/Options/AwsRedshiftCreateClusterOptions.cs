using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-cluster")]
public record AwsRedshiftCreateClusterOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier,
[property: CliOption("--node-type")] string NodeType,
[property: CliOption("--master-username")] string MasterUsername
) : AwsOptions
{
    [CliOption("--db-name")]
    public string? DbName { get; set; }

    [CliOption("--cluster-type")]
    public string? ClusterType { get; set; }

    [CliOption("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CliOption("--cluster-security-groups")]
    public string[]? ClusterSecurityGroups { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--cluster-subnet-group-name")]
    public string? ClusterSubnetGroupName { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--cluster-parameter-group-name")]
    public string? ClusterParameterGroupName { get; set; }

    [CliOption("--automated-snapshot-retention-period")]
    public int? AutomatedSnapshotRetentionPeriod { get; set; }

    [CliOption("--manual-snapshot-retention-period")]
    public int? ManualSnapshotRetentionPeriod { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--cluster-version")]
    public string? ClusterVersion { get; set; }

    [CliOption("--number-of-nodes")]
    public int? NumberOfNodes { get; set; }

    [CliOption("--hsm-client-certificate-identifier")]
    public string? HsmClientCertificateIdentifier { get; set; }

    [CliOption("--hsm-configuration-identifier")]
    public string? HsmConfigurationIdentifier { get; set; }

    [CliOption("--elastic-ip")]
    public string? ElasticIp { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--additional-info")]
    public string? AdditionalInfo { get; set; }

    [CliOption("--iam-roles")]
    public string[]? IamRoles { get; set; }

    [CliOption("--maintenance-track-name")]
    public string? MaintenanceTrackName { get; set; }

    [CliOption("--snapshot-schedule-identifier")]
    public string? SnapshotScheduleIdentifier { get; set; }

    [CliOption("--aqua-configuration-status")]
    public string? AquaConfigurationStatus { get; set; }

    [CliOption("--default-iam-role-arn")]
    public string? DefaultIamRoleArn { get; set; }

    [CliOption("--load-sample-data")]
    public string? LoadSampleData { get; set; }

    [CliOption("--master-password-secret-kms-key-id")]
    public string? MasterPasswordSecretKmsKeyId { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--redshift-idc-application-arn")]
    public string? RedshiftIdcApplicationArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}