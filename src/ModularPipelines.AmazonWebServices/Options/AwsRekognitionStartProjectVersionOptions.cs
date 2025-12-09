using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "start-project-version")]
public record AwsRekognitionStartProjectVersionOptions(
[property: CliOption("--project-version-arn")] string ProjectVersionArn,
[property: CliOption("--min-inference-units")] int MinInferenceUnits
) : AwsOptions
{
    [CliOption("--max-inference-units")]
    public int? MaxInferenceUnits { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}