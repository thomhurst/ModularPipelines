using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "stop-project-version")]
public record AwsRekognitionStopProjectVersionOptions(
[property: CliOption("--project-version-arn")] string ProjectVersionArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}