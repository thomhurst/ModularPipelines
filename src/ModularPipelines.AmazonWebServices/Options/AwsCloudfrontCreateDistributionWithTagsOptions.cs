using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-distribution-with-tags")]
public record AwsCloudfrontCreateDistributionWithTagsOptions(
[property: CliOption("--distribution-config-with-tags")] string DistributionConfigWithTags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}