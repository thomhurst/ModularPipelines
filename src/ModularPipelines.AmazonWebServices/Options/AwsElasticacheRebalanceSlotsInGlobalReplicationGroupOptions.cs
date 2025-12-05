using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "rebalance-slots-in-global-replication-group")]
public record AwsElasticacheRebalanceSlotsInGlobalReplicationGroupOptions(
[property: CliOption("--global-replication-group-id")] string GlobalReplicationGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}