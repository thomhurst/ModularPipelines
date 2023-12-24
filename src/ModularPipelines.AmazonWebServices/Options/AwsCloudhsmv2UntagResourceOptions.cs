using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudhsmv2", "untag-resource")]
public record AwsCloudhsmv2UntagResourceOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--tag-key-list")] string[] TagKeyList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}