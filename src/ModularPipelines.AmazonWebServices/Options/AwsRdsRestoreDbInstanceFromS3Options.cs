using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "restore-db-instance-from-s3")]
public record AwsRdsRestoreDbInstanceFromS3Options(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CliOption("--db-instance-class")] string DbInstanceClass,
[property: CliOption("--engine")] string Engine,
[property: CliOption("--source-engine")] string SourceEngine,
[property: CliOption("--source-engine-version")] string SourceEngineVersion,
[property: CliOption("--s3-bucket-name")] string S3BucketName,
[property: CliOption("--s3-ingestion-role-arn")] string S3IngestionRoleArn
) : AwsOptions
{
    [CliOption("--db-name")]
    public string? DbName { get; set; }

    [CliOption("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CliOption("--master-username")]
    public string? MasterUsername { get; set; }

    [CliOption("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CliOption("--db-security-groups")]
    public string[]? DbSecurityGroups { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--db-parameter-group-name")]
    public string? DbParameterGroupName { get; set; }

    [CliOption("--backup-retention-period")]
    public int? BackupRetentionPeriod { get; set; }

    [CliOption("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--license-model")]
    public string? LicenseModel { get; set; }

    [CliOption("--iops")]
    public int? Iops { get; set; }

    [CliOption("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--monitoring-interval")]
    public int? MonitoringInterval { get; set; }

    [CliOption("--monitoring-role-arn")]
    public string? MonitoringRoleArn { get; set; }

    [CliOption("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CliOption("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CliOption("--performance-insights-retention-period")]
    public int? PerformanceInsightsRetentionPeriod { get; set; }

    [CliOption("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CliOption("--processor-features")]
    public string[]? ProcessorFeatures { get; set; }

    [CliOption("--max-allocated-storage")]
    public int? MaxAllocatedStorage { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--storage-throughput")]
    public int? StorageThroughput { get; set; }

    [CliOption("--master-user-secret-kms-key-id")]
    public string? MasterUserSecretKmsKeyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}