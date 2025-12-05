using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "change-tags-for-resource")]
public record AwsRoute53ChangeTagsForResourceOptions(
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--add-tags")]
    public string[]? AddTags { get; set; }

    [CliOption("--remove-tag-keys")]
    public string[]? RemoveTagKeys { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}