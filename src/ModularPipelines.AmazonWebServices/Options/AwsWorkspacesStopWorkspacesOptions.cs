using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "stop-workspaces")]
public record AwsWorkspacesStopWorkspacesOptions(
[property: CommandSwitch("--stop-workspace-requests")] string[] StopWorkspaceRequests
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}