using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "add-tags-to-resource")]
public record AwsSsmAddTagsToResourceOptions(
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--tags")] string[] Tags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}