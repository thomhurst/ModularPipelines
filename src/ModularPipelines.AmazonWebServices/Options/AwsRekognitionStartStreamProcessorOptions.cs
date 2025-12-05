using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "start-stream-processor")]
public record AwsRekognitionStartStreamProcessorOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--start-selector")]
    public string? StartSelector { get; set; }

    [CliOption("--stop-selector")]
    public string? StopSelector { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}