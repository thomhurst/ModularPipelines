using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "add-tags-to-resource")]
public record AwsSsmAddTagsToResourceOptions(
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--tags")] string[] Tags
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}