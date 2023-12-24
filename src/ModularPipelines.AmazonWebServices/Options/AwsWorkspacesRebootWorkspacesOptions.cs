using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "reboot-workspaces")]
public record AwsWorkspacesRebootWorkspacesOptions(
[property: CommandSwitch("--reboot-workspace-requests")] string[] RebootWorkspaceRequests
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}