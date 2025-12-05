using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "reset-cache-parameter-group")]
public record AwsElasticacheResetCacheParameterGroupOptions(
[property: CliOption("--cache-parameter-group-name")] string CacheParameterGroupName
) : AwsOptions
{
    [CliOption("--parameter-name-values")]
    public string[]? ParameterNameValues { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}