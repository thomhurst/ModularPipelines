using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "create-api-cache")]
public record AwsAppsyncCreateApiCacheOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--ttl")] long Ttl,
[property: CliOption("--api-caching-behavior")] string ApiCachingBehavior,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}