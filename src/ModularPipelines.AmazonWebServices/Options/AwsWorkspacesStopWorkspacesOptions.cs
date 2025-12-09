using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "stop-workspaces")]
public record AwsWorkspacesStopWorkspacesOptions(
[property: CliOption("--stop-workspace-requests")] string[] StopWorkspaceRequests
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}