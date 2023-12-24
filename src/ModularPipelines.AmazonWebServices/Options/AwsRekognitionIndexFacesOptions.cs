using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "index-faces")]
public record AwsRekognitionIndexFacesOptions(
[property: CommandSwitch("--collection-id")] string CollectionId
) : AwsOptions
{
    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--external-image-id")]
    public string? ExternalImageId { get; set; }

    [CommandSwitch("--detection-attributes")]
    public string[]? DetectionAttributes { get; set; }

    [CommandSwitch("--max-faces")]
    public int? MaxFaces { get; set; }

    [CommandSwitch("--quality-filter")]
    public string? QualityFilter { get; set; }

    [CommandSwitch("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}