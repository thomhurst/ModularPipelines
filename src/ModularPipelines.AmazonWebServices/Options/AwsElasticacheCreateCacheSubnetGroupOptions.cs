using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "create-cache-subnet-group")]
public record AwsElasticacheCreateCacheSubnetGroupOptions(
[property: CliOption("--cache-subnet-group-name")] string CacheSubnetGroupName,
[property: CliOption("--cache-subnet-group-description")] string CacheSubnetGroupDescription,
[property: CliOption("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}