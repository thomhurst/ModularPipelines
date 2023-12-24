using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dax", "untag-resource")]
public record AwsDaxUntagResourceOptions(
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--tag-keys")] string[] TagKeys
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}