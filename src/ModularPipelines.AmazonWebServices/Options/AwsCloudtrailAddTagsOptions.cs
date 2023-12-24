using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "add-tags")]
public record AwsCloudtrailAddTagsOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--tags-list")] string[] TagsList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}