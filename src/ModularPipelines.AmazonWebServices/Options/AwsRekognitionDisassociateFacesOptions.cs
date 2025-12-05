using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "disassociate-faces")]
public record AwsRekognitionDisassociateFacesOptions(
[property: CliOption("--collection-id")] string CollectionId,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--face-ids")] string[] FaceIds
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}