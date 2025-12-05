using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "rebuild-workspaces")]
public record AwsWorkspacesRebuildWorkspacesOptions(
[property: CliOption("--rebuild-workspace-requests")] string[] RebuildWorkspaceRequests
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}