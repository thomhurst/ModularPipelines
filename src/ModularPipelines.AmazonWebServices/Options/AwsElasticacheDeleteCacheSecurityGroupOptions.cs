using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "delete-cache-security-group")]
public record AwsElasticacheDeleteCacheSecurityGroupOptions(
[property: CliOption("--cache-security-group-name")] string CacheSecurityGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}