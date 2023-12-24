using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudhsmv2", "tag-resource")]
public record AwsCloudhsmv2TagResourceOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--tag-list")] string[] TagList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}