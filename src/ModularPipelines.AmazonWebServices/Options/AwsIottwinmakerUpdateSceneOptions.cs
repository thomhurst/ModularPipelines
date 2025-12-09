using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "update-scene")]
public record AwsIottwinmakerUpdateSceneOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--scene-id")] string SceneId
) : AwsOptions
{
    [CliOption("--content-location")]
    public string? ContentLocation { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CliOption("--scene-metadata")]
    public IEnumerable<KeyValue>? SceneMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}