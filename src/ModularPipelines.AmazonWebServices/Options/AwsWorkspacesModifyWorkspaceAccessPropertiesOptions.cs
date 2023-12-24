using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "modify-workspace-access-properties")]
public record AwsWorkspacesModifyWorkspaceAccessPropertiesOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--workspace-access-properties")] string WorkspaceAccessProperties
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}