using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "modify-workspace-creation-properties")]
public record AwsWorkspacesModifyWorkspaceCreationPropertiesOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--workspace-creation-properties")] string WorkspaceCreationProperties
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}