using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "start-project-version")]
public record AwsRekognitionStartProjectVersionOptions(
[property: CommandSwitch("--project-version-arn")] string ProjectVersionArn,
[property: CommandSwitch("--min-inference-units")] int MinInferenceUnits
) : AwsOptions
{
    [CommandSwitch("--max-inference-units")]
    public int? MaxInferenceUnits { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}