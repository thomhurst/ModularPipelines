using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "start-db-instance-automated-backups-replication")]
public record AwsRdsStartDbInstanceAutomatedBackupsReplicationOptions(
[property: CliOption("--source-db-instance-arn")] string SourceDbInstanceArn
) : AwsOptions
{
    [CliOption("--backup-retention-period")]
    public int? BackupRetentionPeriod { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--pre-signed-url")]
    public string? PreSignedUrl { get; set; }

    [CliOption("--source-region")]
    public string? SourceRegion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}