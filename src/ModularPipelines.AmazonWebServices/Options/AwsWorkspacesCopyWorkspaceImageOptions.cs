using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "copy-workspace-image")]
public record AwsWorkspacesCopyWorkspaceImageOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--source-image-id")] string SourceImageId,
[property: CliOption("--source-region")] string SourceRegion
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}