using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-distribution-with-tags")]
public record AwsCloudfrontCreateDistributionWithTagsOptions(
[property: CommandSwitch("--distribution-config-with-tags")] string DistributionConfigWithTags
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}