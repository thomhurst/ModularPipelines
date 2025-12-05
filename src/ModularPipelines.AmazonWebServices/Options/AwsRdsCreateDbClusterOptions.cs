using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-db-cluster")]
public record AwsRdsCreateDbClusterOptions(
[property: CliOption("--db-cluster-identifier")] string DbClusterIdentifier,
[property: CliOption("--engine")] string Engine
) : AwsOptions
{
    [CliOption("--availability-zones")]
    public string[]? AvailabilityZones { get; set; }

    [CliOption("--backup-retention-period")]
    public int? BackupRetentionPeriod { get; set; }

    [CliOption("--character-set-name")]
    public string? CharacterSetName { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--db-cluster-parameter-group-name")]
    public string? DbClusterParameterGroupName { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--master-username")]
    public string? MasterUsername { get; set; }

    [CliOption("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CliOption("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CliOption("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--replication-source-identifier")]
    public string? ReplicationSourceIdentifier { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--pre-signed-url")]
    public string? PreSignedUrl { get; set; }

    [CliOption("--backtrack-window")]
    public long? BacktrackWindow { get; set; }

    [CliOption("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CliOption("--engine-mode")]
    public string? EngineMode { get; set; }

    [CliOption("--scaling-configuration")]
    public string? ScalingConfiguration { get; set; }

    [CliOption("--rds-custom-cluster-configuration")]
    public string? RdsCustomClusterConfiguration { get; set; }

    [CliOption("--global-cluster-identifier")]
    public string? GlobalClusterIdentifier { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CliOption("--db-cluster-instance-class")]
    public string? DbClusterInstanceClass { get; set; }

    [CliOption("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--iops")]
    public int? Iops { get; set; }

    [CliOption("--monitoring-interval")]
    public int? MonitoringInterval { get; set; }

    [CliOption("--monitoring-role-arn")]
    public string? MonitoringRoleArn { get; set; }

    [CliOption("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CliOption("--performance-insights-retention-period")]
    public int? PerformanceInsightsRetentionPeriod { get; set; }

    [CliOption("--serverless-v2-scaling-configuration")]
    public string? ServerlessV2ScalingConfiguration { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--db-system-id")]
    public string? DbSystemId { get; set; }

    [CliOption("--master-user-secret-kms-key-id")]
    public string? MasterUserSecretKmsKeyId { get; set; }

    [CliOption("--source-region")]
    public string? SourceRegion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}