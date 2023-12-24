using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "search-faces")]
public record AwsRekognitionSearchFacesOptions(
[property: CommandSwitch("--collection-id")] string CollectionId,
[property: CommandSwitch("--face-id")] string FaceId
) : AwsOptions
{
    [CommandSwitch("--max-faces")]
    public int? MaxFaces { get; set; }

    [CommandSwitch("--face-match-threshold")]
    public float? FaceMatchThreshold { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}