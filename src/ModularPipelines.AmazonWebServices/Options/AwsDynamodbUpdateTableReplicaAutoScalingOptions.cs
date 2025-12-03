using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "update-table-replica-auto-scaling")]
public record AwsDynamodbUpdateTableReplicaAutoScalingOptions(
[property: CliOption("--table-name")] string TableName
) : AwsOptions
{
    [CliOption("--global-secondary-index-updates")]
    public string[]? GlobalSecondaryIndexUpdates { get; set; }

    [CliOption("--provisioned-write-capacity-auto-scaling-update")]
    public string? ProvisionedWriteCapacityAutoScalingUpdate { get; set; }

    [CliOption("--replica-updates")]
    public string[]? ReplicaUpdates { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}