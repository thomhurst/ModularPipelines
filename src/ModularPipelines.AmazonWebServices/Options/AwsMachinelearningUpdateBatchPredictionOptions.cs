using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "update-batch-prediction")]
public record AwsMachinelearningUpdateBatchPredictionOptions(
[property: CliOption("--batch-prediction-id")] string BatchPredictionId,
[property: CliOption("--batch-prediction-name")] string BatchPredictionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}