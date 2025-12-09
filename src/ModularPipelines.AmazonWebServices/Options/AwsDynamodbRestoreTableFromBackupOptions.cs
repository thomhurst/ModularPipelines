using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "restore-table-from-backup")]
public record AwsDynamodbRestoreTableFromBackupOptions(
[property: CliOption("--target-table-name")] string TargetTableName,
[property: CliOption("--backup-arn")] string BackupArn
) : AwsOptions
{
    [CliOption("--billing-mode-override")]
    public string? BillingModeOverride { get; set; }

    [CliOption("--global-secondary-index-override")]
    public string[]? GlobalSecondaryIndexOverride { get; set; }

    [CliOption("--local-secondary-index-override")]
    public string[]? LocalSecondaryIndexOverride { get; set; }

    [CliOption("--provisioned-throughput-override")]
    public string? ProvisionedThroughputOverride { get; set; }

    [CliOption("--sse-specification-override")]
    public string? SseSpecificationOverride { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}