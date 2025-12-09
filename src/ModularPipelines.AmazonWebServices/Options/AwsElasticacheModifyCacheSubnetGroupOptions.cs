using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "modify-cache-subnet-group")]
public record AwsElasticacheModifyCacheSubnetGroupOptions(
[property: CliOption("--cache-subnet-group-name")] string CacheSubnetGroupName
) : AwsOptions
{
    [CliOption("--cache-subnet-group-description")]
    public string? CacheSubnetGroupDescription { get; set; }

    [CliOption("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}