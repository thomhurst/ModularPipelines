using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "test-migration")]
public record AwsElasticacheTestMigrationOptions(
[property: CliOption("--replication-group-id")] string ReplicationGroupId,
[property: CliOption("--customer-node-endpoint-list")] string[] CustomerNodeEndpointList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}