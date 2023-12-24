using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "rebuild-workspaces")]
public record AwsWorkspacesRebuildWorkspacesOptions(
[property: CommandSwitch("--rebuild-workspace-requests")] string[] RebuildWorkspaceRequests
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}