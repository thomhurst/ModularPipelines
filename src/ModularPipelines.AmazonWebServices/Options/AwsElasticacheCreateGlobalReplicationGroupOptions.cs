using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "create-global-replication-group")]
public record AwsElasticacheCreateGlobalReplicationGroupOptions(
[property: CliOption("--global-replication-group-id-suffix")] string GlobalReplicationGroupIdSuffix,
[property: CliOption("--primary-replication-group-id")] string PrimaryReplicationGroupId
) : AwsOptions
{
    [CliOption("--global-replication-group-description")]
    public string? GlobalReplicationGroupDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}