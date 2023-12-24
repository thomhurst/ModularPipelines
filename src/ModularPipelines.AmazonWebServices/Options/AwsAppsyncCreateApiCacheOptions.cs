using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "create-api-cache")]
public record AwsAppsyncCreateApiCacheOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--ttl")] long Ttl,
[property: CommandSwitch("--api-caching-behavior")] string ApiCachingBehavior,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}