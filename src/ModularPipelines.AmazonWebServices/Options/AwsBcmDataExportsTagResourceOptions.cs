using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bcm-data-exports", "tag-resource")]
public record AwsBcmDataExportsTagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--resource-tags")] string[] ResourceTags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}