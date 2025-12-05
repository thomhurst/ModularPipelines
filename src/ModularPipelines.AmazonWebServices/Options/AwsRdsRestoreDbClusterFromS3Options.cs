using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "restore-db-cluster-from-s3")]
public record AwsRdsRestoreDbClusterFromS3Options(
[property: CliOption("--db-cluster-identifier")] string DbClusterIdentifier,
[property: CliOption("--engine")] string Engine,
[property: CliOption("--master-username")] string MasterUsername,
[property: CliOption("--source-engine")] string SourceEngine,
[property: CliOption("--source-engine-version")] string SourceEngineVersion,
[property: CliOption("--s3-bucket-name")] string S3BucketName,
[property: CliOption("--s3-ingestion-role-arn")] string S3IngestionRoleArn
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

    [CliOption("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CliOption("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CliOption("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CliOption("--backtrack-window")]
    public long? BacktrackWindow { get; set; }

    [CliOption("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CliOption("--serverless-v2-scaling-configuration")]
    public string? ServerlessV2ScalingConfiguration { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--master-user-secret-kms-key-id")]
    public string? MasterUserSecretKmsKeyId { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}