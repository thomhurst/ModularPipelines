using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "create-batch-prediction")]
public record AwsMachinelearningCreateBatchPredictionOptions(
[property: CliOption("--batch-prediction-id")] string BatchPredictionId,
[property: CliOption("--ml-model-id")] string MlModelId,
[property: CliOption("--batch-prediction-data-source-id")] string BatchPredictionDataSourceId,
[property: CliOption("--output-uri")] string OutputUri
) : AwsOptions
{
    [CliOption("--batch-prediction-name")]
    public string? BatchPredictionName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}