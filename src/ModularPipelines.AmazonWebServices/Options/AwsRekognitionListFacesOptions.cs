using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "list-faces")]
public record AwsRekognitionListFacesOptions(
[property: CommandSwitch("--collection-id")] string CollectionId
) : AwsOptions
{
    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }

    [CommandSwitch("--face-ids")]
    public string[]? FaceIds { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}