using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "delete-global-replication-group")]
public record AwsElasticacheDeleteGlobalReplicationGroupOptions(
[property: CliOption("--global-replication-group-id")] string GlobalReplicationGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}