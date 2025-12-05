using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "modify-global-replication-group")]
public record AwsElasticacheModifyGlobalReplicationGroupOptions(
[property: CliOption("--global-replication-group-id")] string GlobalReplicationGroupId
) : AwsOptions
{
    [CliOption("--cache-node-type")]
    public string? CacheNodeType { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--cache-parameter-group-name")]
    public string? CacheParameterGroupName { get; set; }

    [CliOption("--global-replication-group-description")]
    public string? GlobalReplicationGroupDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}