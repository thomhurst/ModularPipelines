using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "copy-workspace-image")]
public record AwsWorkspacesCopyWorkspaceImageOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--source-image-id")] string SourceImageId,
[property: CommandSwitch("--source-region")] string SourceRegion
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}