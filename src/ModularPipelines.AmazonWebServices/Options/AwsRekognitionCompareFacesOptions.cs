using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "compare-faces")]
public record AwsRekognitionCompareFacesOptions : AwsOptions
{
    [CliOption("--source-image")]
    public string? SourceImage { get; set; }

    [CliOption("--target-image")]
    public string? TargetImage { get; set; }

    [CliOption("--similarity-threshold")]
    public float? SimilarityThreshold { get; set; }

    [CliOption("--quality-filter")]
    public string? QualityFilter { get; set; }

    [CliOption("--source-image-bytes")]
    public string? SourceImageBytes { get; set; }

    [CliOption("--target-image-bytes")]
    public string? TargetImageBytes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}