using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "untag-resource")]
public record AwsCeUntagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--resource-tag-keys")] string[] ResourceTagKeys
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}