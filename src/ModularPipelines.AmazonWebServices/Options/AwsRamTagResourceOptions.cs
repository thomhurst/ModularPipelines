using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "tag-resource")]
public record AwsRamTagResourceOptions(
[property: CliOption("--tags")] string[] Tags
) : AwsOptions
{
    [CliOption("--resource-share-arn")]
    public string? ResourceShareArn { get; set; }

    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}