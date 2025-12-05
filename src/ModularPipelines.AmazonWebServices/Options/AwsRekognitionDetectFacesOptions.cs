using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "detect-faces")]
public record AwsRekognitionDetectFacesOptions : AwsOptions
{
    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--attributes")]
    public string[]? Attributes { get; set; }

    [CliOption("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}