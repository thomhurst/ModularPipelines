using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "restore-db-cluster-from-s3")]
public record AwsRdsRestoreDbClusterFromS3Options(
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier,
[property: CommandSwitch("--engine")] string Engine,
[property: CommandSwitch("--master-username")] string MasterUsername,
[property: CommandSwitch("--source-engine")] string SourceEngine,
[property: CommandSwitch("--source-engine-version")] string SourceEngineVersion,
[property: CommandSwitch("--s3-bucket-name")] string S3BucketName,
[property: CommandSwitch("--s3-ingestion-role-arn")] string S3IngestionRoleArn
) : AwsOptions
{
    [CommandSwitch("--availability-zones")]
    public string[]? AvailabilityZones { get; set; }

    [CommandSwitch("--backup-retention-period")]
    public int? BackupRetentionPeriod { get; set; }

    [CommandSwitch("--character-set-name")]
    public string? CharacterSetName { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--db-cluster-parameter-group-name")]
    public string? DbClusterParameterGroupName { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CommandSwitch("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CommandSwitch("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CommandSwitch("--backtrack-window")]
    public long? BacktrackWindow { get; set; }

    [CommandSwitch("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CommandSwitch("--serverless-v2-scaling-configuration")]
    public string? ServerlessV2ScalingConfiguration { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--master-user-secret-kms-key-id")]
    public string? MasterUserSecretKmsKeyId { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}