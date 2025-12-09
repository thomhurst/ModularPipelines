using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "delete-db-instance-automated-backup")]
public record AwsRdsDeleteDbInstanceAutomatedBackupOptions : AwsOptions
{
    [CliOption("--dbi-resource-id")]
    public string? DbiResourceId { get; set; }

    [CliOption("--db-instance-automated-backups-arn")]
    public string? DbInstanceAutomatedBackupsArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}