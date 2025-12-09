using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "delete-scene")]
public record AwsIottwinmakerDeleteSceneOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--scene-id")] string SceneId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}