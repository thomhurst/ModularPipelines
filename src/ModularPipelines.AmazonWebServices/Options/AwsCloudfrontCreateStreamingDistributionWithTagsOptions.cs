using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-streaming-distribution-with-tags")]
public record AwsCloudfrontCreateStreamingDistributionWithTagsOptions(
[property: CommandSwitch("--streaming-distribution-config-with-tags")] string StreamingDistributionConfigWithTags
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}