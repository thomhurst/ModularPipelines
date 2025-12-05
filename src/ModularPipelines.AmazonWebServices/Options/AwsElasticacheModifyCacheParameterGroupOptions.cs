using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "modify-cache-parameter-group")]
public record AwsElasticacheModifyCacheParameterGroupOptions(
[property: CliOption("--cache-parameter-group-name")] string CacheParameterGroupName,
[property: CliOption("--parameter-name-values")] string[] ParameterNameValues
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}