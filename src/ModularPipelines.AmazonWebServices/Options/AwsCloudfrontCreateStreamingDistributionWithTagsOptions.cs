using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-streaming-distribution-with-tags")]
public record AwsCloudfrontCreateStreamingDistributionWithTagsOptions(
[property: CliOption("--streaming-distribution-config-with-tags")] string StreamingDistributionConfigWithTags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}