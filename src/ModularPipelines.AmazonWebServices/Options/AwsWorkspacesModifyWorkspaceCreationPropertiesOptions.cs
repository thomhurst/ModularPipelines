using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "modify-workspace-creation-properties")]
public record AwsWorkspacesModifyWorkspaceCreationPropertiesOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--workspace-creation-properties")] string WorkspaceCreationProperties
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}