using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "associate-faces")]
public record AwsRekognitionAssociateFacesOptions(
[property: CommandSwitch("--collection-id")] string CollectionId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--face-ids")] string[] FaceIds
) : AwsOptions
{
    [CommandSwitch("--user-match-threshold")]
    public float? UserMatchThreshold { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}