using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "update-table-replica-auto-scaling")]
public record AwsDynamodbUpdateTableReplicaAutoScalingOptions(
[property: CommandSwitch("--table-name")] string TableName
) : AwsOptions
{
    [CommandSwitch("--global-secondary-index-updates")]
    public string[]? GlobalSecondaryIndexUpdates { get; set; }

    [CommandSwitch("--provisioned-write-capacity-auto-scaling-update")]
    public string? ProvisionedWriteCapacityAutoScalingUpdate { get; set; }

    [CommandSwitch("--replica-updates")]
    public string[]? ReplicaUpdates { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}