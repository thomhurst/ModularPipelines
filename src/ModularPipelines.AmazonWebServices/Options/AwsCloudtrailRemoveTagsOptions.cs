using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "remove-tags")]
public record AwsCloudtrailRemoveTagsOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--tags-list")] string[] TagsList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}