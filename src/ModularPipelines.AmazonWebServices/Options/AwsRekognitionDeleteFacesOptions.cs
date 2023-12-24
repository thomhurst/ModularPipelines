using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "delete-faces")]
public record AwsRekognitionDeleteFacesOptions(
[property: CommandSwitch("--collection-id")] string CollectionId,
[property: CommandSwitch("--face-ids")] string[] FaceIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}