using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fms", "tag-resource")]
public record AwsFmsTagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--tag-list")] string[] TagList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}