using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "start-db-instance-automated-backups-replication")]
public record AwsRdsStartDbInstanceAutomatedBackupsReplicationOptions(
[property: CommandSwitch("--source-db-instance-arn")] string SourceDbInstanceArn
) : AwsOptions
{
    [CommandSwitch("--backup-retention-period")]
    public int? BackupRetentionPeriod { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--pre-signed-url")]
    public string? PreSignedUrl { get; set; }

    [CommandSwitch("--source-region")]
    public string? SourceRegion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}