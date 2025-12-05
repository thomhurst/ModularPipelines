using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "create-cache-parameter-group")]
public record AwsElasticacheCreateCacheParameterGroupOptions(
[property: CliOption("--cache-parameter-group-name")] string CacheParameterGroupName,
[property: CliOption("--cache-parameter-group-family")] string CacheParameterGroupFamily,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}