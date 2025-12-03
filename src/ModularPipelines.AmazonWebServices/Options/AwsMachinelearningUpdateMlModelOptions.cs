using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "update-ml-model")]
public record AwsMachinelearningUpdateMlModelOptions(
[property: CliOption("--ml-model-id")] string MlModelId
) : AwsOptions
{
    [CliOption("--ml-model-name")]
    public string? MlModelName { get; set; }

    [CliOption("--score-threshold")]
    public float? ScoreThreshold { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}