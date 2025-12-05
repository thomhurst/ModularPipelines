using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "delete-faces")]
public record AwsRekognitionDeleteFacesOptions(
[property: CliOption("--collection-id")] string CollectionId,
[property: CliOption("--face-ids")] string[] FaceIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}