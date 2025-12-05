using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "delete-cache-subnet-group")]
public record AwsElasticacheDeleteCacheSubnetGroupOptions(
[property: CliOption("--cache-subnet-group-name")] string CacheSubnetGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}