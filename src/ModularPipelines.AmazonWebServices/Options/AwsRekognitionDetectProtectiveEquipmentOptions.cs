using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "detect-protective-equipment")]
public record AwsRekognitionDetectProtectiveEquipmentOptions : AwsOptions
{
    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--summarization-attributes")]
    public string? SummarizationAttributes { get; set; }

    [CliOption("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}