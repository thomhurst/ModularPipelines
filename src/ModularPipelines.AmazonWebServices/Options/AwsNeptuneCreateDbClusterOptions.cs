using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "create-db-cluster")]
public record AwsNeptuneCreateDbClusterOptions(
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier,
[property: CommandSwitch("--engine")] string Engine
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

    [CommandSwitch("--master-username")]
    public string? MasterUsername { get; set; }

    [CommandSwitch("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CommandSwitch("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CommandSwitch("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--replication-source-identifier")]
    public string? ReplicationSourceIdentifier { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--pre-signed-url")]
    public string? PreSignedUrl { get; set; }

    [CommandSwitch("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CommandSwitch("--serverless-v2-scaling-configuration")]
    public string? ServerlessV2ScalingConfiguration { get; set; }

    [CommandSwitch("--global-cluster-identifier")]
    public string? GlobalClusterIdentifier { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--source-region")]
    public string? SourceRegion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}