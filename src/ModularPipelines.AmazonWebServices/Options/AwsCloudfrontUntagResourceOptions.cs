using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "untag-resource")]
public record AwsCloudfrontUntagResourceOptions(
[property: CliOption("--resource")] string Resource,
[property: CliOption("--tag-keys")] string TagKeys
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}