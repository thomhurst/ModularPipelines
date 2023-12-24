using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "restore-table-to-point-in-time")]
public record AwsDynamodbRestoreTableToPointInTimeOptions(
[property: CommandSwitch("--target-table-name")] string TargetTableName
) : AwsOptions
{
    [CommandSwitch("--source-table-arn")]
    public string? SourceTableArn { get; set; }

    [CommandSwitch("--source-table-name")]
    public string? SourceTableName { get; set; }

    [CommandSwitch("--restore-date-time")]
    public long? RestoreDateTime { get; set; }

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