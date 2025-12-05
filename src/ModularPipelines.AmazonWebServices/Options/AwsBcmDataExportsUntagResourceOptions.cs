using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bcm-data-exports", "untag-resource")]
public record AwsBcmDataExportsUntagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--resource-tag-keys")] string[] ResourceTagKeys
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}