using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "tag-resource")]
public record AwsRamTagResourceOptions(
[property: CommandSwitch("--tags")] string[] Tags
) : AwsOptions
{
    [CommandSwitch("--resource-share-arn")]
    public string? ResourceShareArn { get; set; }

    [CommandSwitch("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}