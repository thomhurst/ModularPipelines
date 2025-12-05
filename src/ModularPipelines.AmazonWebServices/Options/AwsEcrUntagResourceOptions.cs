using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "untag-resource")]
public record AwsEcrUntagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--tag-keys")] string[] TagKeys
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}