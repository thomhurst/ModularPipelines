using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fis", "untag-resource")]
public record AwsFisUntagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CliOption("--tag-keys")]
    public string[]? TagKeys { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}