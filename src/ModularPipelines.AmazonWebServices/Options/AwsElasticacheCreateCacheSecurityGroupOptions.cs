using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "create-cache-security-group")]
public record AwsElasticacheCreateCacheSecurityGroupOptions(
[property: CliOption("--cache-security-group-name")] string CacheSecurityGroupName,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}