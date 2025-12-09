using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "disassociate-global-replication-group")]
public record AwsElasticacheDisassociateGlobalReplicationGroupOptions(
[property: CliOption("--global-replication-group-id")] string GlobalReplicationGroupId,
[property: CliOption("--replication-group-id")] string ReplicationGroupId,
[property: CliOption("--replication-group-region")] string ReplicationGroupRegion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}