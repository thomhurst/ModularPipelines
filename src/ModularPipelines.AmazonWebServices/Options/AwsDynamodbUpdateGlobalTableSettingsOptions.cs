using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "update-global-table-settings")]
public record AwsDynamodbUpdateGlobalTableSettingsOptions(
[property: CommandSwitch("--global-table-name")] string GlobalTableName
) : AwsOptions
{
    [CommandSwitch("--global-table-billing-mode")]
    public string? GlobalTableBillingMode { get; set; }

    [CommandSwitch("--global-table-provisioned-write-capacity-units")]
    public long? GlobalTableProvisionedWriteCapacityUnits { get; set; }

    [CommandSwitch("--global-table-provisioned-write-capacity-auto-scaling-settings-update")]
    public string? GlobalTableProvisionedWriteCapacityAutoScalingSettingsUpdate { get; set; }

    [CommandSwitch("--global-table-global-secondary-index-settings-update")]
    public string[]? GlobalTableGlobalSecondaryIndexSettingsUpdate { get; set; }

    [CommandSwitch("--replica-settings-update")]
    public string[]? ReplicaSettingsUpdate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}