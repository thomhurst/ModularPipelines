using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "reboot-workspaces")]
public record AwsWorkspacesRebootWorkspacesOptions(
[property: CliOption("--reboot-workspace-requests")] string[] RebootWorkspaceRequests
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}