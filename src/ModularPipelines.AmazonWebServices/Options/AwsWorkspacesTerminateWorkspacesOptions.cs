using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "terminate-workspaces")]
public record AwsWorkspacesTerminateWorkspacesOptions(
[property: CommandSwitch("--terminate-workspace-requests")] string[] TerminateWorkspaceRequests
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}