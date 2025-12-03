using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "modify-workspace-properties")]
public record AwsWorkspacesModifyWorkspacePropertiesOptions(
[property: CliOption("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CliOption("--workspace-properties")]
    public string? WorkspaceProperties { get; set; }

    [CliOption("--data-replication")]
    public string? DataReplication { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}