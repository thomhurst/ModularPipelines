using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "update-scene")]
public record AwsIottwinmakerUpdateSceneOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId,
[property: CommandSwitch("--scene-id")] string SceneId
) : AwsOptions
{
    [CommandSwitch("--content-location")]
    public string? ContentLocation { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CommandSwitch("--scene-metadata")]
    public IEnumerable<KeyValue>? SceneMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}