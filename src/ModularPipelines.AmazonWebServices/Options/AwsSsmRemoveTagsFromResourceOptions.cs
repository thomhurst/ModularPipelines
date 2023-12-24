using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "remove-tags-from-resource")]
public record AwsSsmRemoveTagsFromResourceOptions(
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--tag-keys")] string[] TagKeys
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}