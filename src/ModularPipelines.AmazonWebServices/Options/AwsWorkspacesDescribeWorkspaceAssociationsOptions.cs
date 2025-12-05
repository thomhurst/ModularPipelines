using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "describe-workspace-associations")]
public record AwsWorkspacesDescribeWorkspaceAssociationsOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--associated-resource-types")] string[] AssociatedResourceTypes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}