using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "recognize-celebrities")]
public record AwsRekognitionRecognizeCelebritiesOptions : AwsOptions
{
    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}