using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "create-updated-workspace-image")]
public record AwsWorkspacesCreateUpdatedWorkspaceImageOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--description")] string Description,
[property: CliOption("--source-image-id")] string SourceImageId
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}