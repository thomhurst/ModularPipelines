using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "test-failover")]
public record AwsElasticacheTestFailoverOptions(
[property: CliOption("--replication-group-id")] string ReplicationGroupId,
[property: CliOption("--node-group-id")] string NodeGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}