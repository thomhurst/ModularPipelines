using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "change-tags-for-resource")]
public record AwsRoute53ChangeTagsForResourceOptions(
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--resource-id")] string ResourceId
) : AwsOptions
{
    [CommandSwitch("--add-tags")]
    public string[]? AddTags { get; set; }

    [CommandSwitch("--remove-tag-keys")]
    public string[]? RemoveTagKeys { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}