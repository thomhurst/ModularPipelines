using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "modify-workspace-state")]
public record AwsWorkspacesModifyWorkspaceStateOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--workspace-state")] string WorkspaceState
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}