using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "restore-db-instance-from-s3")]
public record AwsRdsRestoreDbInstanceFromS3Options(
[property: CommandSwitch("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CommandSwitch("--db-instance-class")] string DbInstanceClass,
[property: CommandSwitch("--engine")] string Engine,
[property: CommandSwitch("--source-engine")] string SourceEngine,
[property: CommandSwitch("--source-engine-version")] string SourceEngineVersion,
[property: CommandSwitch("--s3-bucket-name")] string S3BucketName,
[property: CommandSwitch("--s3-ingestion-role-arn")] string S3IngestionRoleArn
) : AwsOptions
{
    [CommandSwitch("--db-name")]
    public string? DbName { get; set; }

    [CommandSwitch("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CommandSwitch("--master-username")]
    public string? MasterUsername { get; set; }

    [CommandSwitch("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CommandSwitch("--db-security-groups")]
    public string[]? DbSecurityGroups { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--db-parameter-group-name")]
    public string? DbParameterGroupName { get; set; }

    [CommandSwitch("--backup-retention-period")]
    public int? BackupRetentionPeriod { get; set; }

    [CommandSwitch("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--license-model")]
    public string? LicenseModel { get; set; }

    [CommandSwitch("--iops")]
    public int? Iops { get; set; }

    [CommandSwitch("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--monitoring-interval")]
    public int? MonitoringInterval { get; set; }

    [CommandSwitch("--monitoring-role-arn")]
    public string? MonitoringRoleArn { get; set; }

    [CommandSwitch("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CommandSwitch("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CommandSwitch("--performance-insights-retention-period")]
    public int? PerformanceInsightsRetentionPeriod { get; set; }

    [CommandSwitch("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CommandSwitch("--processor-features")]
    public string[]? ProcessorFeatures { get; set; }

    [CommandSwitch("--max-allocated-storage")]
    public int? MaxAllocatedStorage { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--storage-throughput")]
    public int? StorageThroughput { get; set; }

    [CommandSwitch("--master-user-secret-kms-key-id")]
    public string? MasterUserSecretKmsKeyId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}