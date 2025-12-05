using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "describe-workspace-snapshots")]
public record AwsWorkspacesDescribeWorkspaceSnapshotsOptions(
[property: CliOption("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}