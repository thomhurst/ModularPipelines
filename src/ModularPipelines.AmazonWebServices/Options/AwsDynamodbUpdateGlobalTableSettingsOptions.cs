using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "update-global-table-settings")]
public record AwsDynamodbUpdateGlobalTableSettingsOptions(
[property: CliOption("--global-table-name")] string GlobalTableName
) : AwsOptions
{
    [CliOption("--global-table-billing-mode")]
    public string? GlobalTableBillingMode { get; set; }

    [CliOption("--global-table-provisioned-write-capacity-units")]
    public long? GlobalTableProvisionedWriteCapacityUnits { get; set; }

    [CliOption("--global-table-provisioned-write-capacity-auto-scaling-settings-update")]
    public string? GlobalTableProvisionedWriteCapacityAutoScalingSettingsUpdate { get; set; }

    [CliOption("--global-table-global-secondary-index-settings-update")]
    public string[]? GlobalTableGlobalSecondaryIndexSettingsUpdate { get; set; }

    [CliOption("--replica-settings-update")]
    public string[]? ReplicaSettingsUpdate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}