using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "remove-tags")]
public record AwsCloudtrailRemoveTagsOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--tags-list")] string[] TagsList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}