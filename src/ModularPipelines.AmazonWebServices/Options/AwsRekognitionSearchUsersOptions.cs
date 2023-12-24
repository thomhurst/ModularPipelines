using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "search-users")]
public record AwsRekognitionSearchUsersOptions(
[property: CommandSwitch("--collection-id")] string CollectionId
) : AwsOptions
{
    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }

    [CommandSwitch("--face-id")]
    public string? FaceId { get; set; }

    [CommandSwitch("--user-match-threshold")]
    public float? UserMatchThreshold { get; set; }

    [CommandSwitch("--max-users")]
    public int? MaxUsers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}