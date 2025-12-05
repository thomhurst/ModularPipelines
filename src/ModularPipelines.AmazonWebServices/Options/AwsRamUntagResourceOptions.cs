using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "untag-resource")]
public record AwsRamUntagResourceOptions(
[property: CliOption("--tag-keys")] string[] TagKeys
) : AwsOptions
{
    [CliOption("--resource-share-arn")]
    public string? ResourceShareArn { get; set; }

    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}