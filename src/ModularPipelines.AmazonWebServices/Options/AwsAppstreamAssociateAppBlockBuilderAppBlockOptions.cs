using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "associate-app-block-builder-app-block")]
public record AwsAppstreamAssociateAppBlockBuilderAppBlockOptions(
[property: CommandSwitch("--app-block-arn")] string AppBlockArn,
[property: CommandSwitch("--app-block-builder-name")] string AppBlockBuilderName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}