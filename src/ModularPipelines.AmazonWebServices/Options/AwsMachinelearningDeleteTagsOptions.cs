using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "delete-tags")]
public record AwsMachinelearningDeleteTagsOptions(
[property: CommandSwitch("--tag-keys")] string[] TagKeys,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}