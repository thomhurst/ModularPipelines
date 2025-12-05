using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "create-scene")]
public record AwsIottwinmakerCreateSceneOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--scene-id")] string SceneId,
[property: CliOption("--content-location")] string ContentLocation
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--scene-metadata")]
    public IEnumerable<KeyValue>? SceneMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}