using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "search-users-by-image")]
public record AwsRekognitionSearchUsersByImageOptions(
[property: CommandSwitch("--collection-id")] string CollectionId
) : AwsOptions
{
    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--user-match-threshold")]
    public float? UserMatchThreshold { get; set; }

    [CommandSwitch("--max-users")]
    public int? MaxUsers { get; set; }

    [CommandSwitch("--quality-filter")]
    public string? QualityFilter { get; set; }

    [CommandSwitch("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}