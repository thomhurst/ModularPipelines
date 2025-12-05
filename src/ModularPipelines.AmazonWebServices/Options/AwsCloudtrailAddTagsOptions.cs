using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "add-tags")]
public record AwsCloudtrailAddTagsOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--tags-list")] string[] TagsList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}