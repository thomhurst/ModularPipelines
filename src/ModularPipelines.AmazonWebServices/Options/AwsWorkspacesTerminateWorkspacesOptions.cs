using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "terminate-workspaces")]
public record AwsWorkspacesTerminateWorkspacesOptions(
[property: CliOption("--terminate-workspace-requests")] string[] TerminateWorkspaceRequests
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}