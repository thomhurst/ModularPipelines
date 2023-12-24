using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "modify-workspace-properties")]
public record AwsWorkspacesModifyWorkspacePropertiesOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CommandSwitch("--workspace-properties")]
    public string? WorkspaceProperties { get; set; }

    [CommandSwitch("--data-replication")]
    public string? DataReplication { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}