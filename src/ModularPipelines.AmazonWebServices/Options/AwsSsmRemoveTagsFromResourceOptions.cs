using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "remove-tags-from-resource")]
public record AwsSsmRemoveTagsFromResourceOptions(
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--tag-keys")] string[] TagKeys
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}