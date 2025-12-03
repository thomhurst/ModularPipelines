using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "modify-workspace-access-properties")]
public record AwsWorkspacesModifyWorkspaceAccessPropertiesOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--workspace-access-properties")] string WorkspaceAccessProperties
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}