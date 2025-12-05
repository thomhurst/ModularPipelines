using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "create-workspace-image")]
public record AwsWorkspacesCreateWorkspaceImageOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--description")] string Description,
[property: CliOption("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}