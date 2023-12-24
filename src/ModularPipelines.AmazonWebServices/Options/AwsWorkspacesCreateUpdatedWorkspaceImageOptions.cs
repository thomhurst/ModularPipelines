using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "create-updated-workspace-image")]
public record AwsWorkspacesCreateUpdatedWorkspaceImageOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--source-image-id")] string SourceImageId
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}