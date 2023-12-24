using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "restore-table-from-backup")]
public record AwsDynamodbRestoreTableFromBackupOptions(
[property: CommandSwitch("--target-table-name")] string TargetTableName,
[property: CommandSwitch("--backup-arn")] string BackupArn
) : AwsOptions
{
    [CommandSwitch("--billing-mode-override")]
    public string? BillingModeOverride { get; set; }

    [CommandSwitch("--global-secondary-index-override")]
    public string[]? GlobalSecondaryIndexOverride { get; set; }

    [CommandSwitch("--local-secondary-index-override")]
    public string[]? LocalSecondaryIndexOverride { get; set; }

    [CommandSwitch("--provisioned-throughput-override")]
    public string? ProvisionedThroughputOverride { get; set; }

    [CommandSwitch("--sse-specification-override")]
    public string? SseSpecificationOverride { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}