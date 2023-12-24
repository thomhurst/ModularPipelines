using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "create-batch-prediction")]
public record AwsMachinelearningCreateBatchPredictionOptions(
[property: CommandSwitch("--batch-prediction-id")] string BatchPredictionId,
[property: CommandSwitch("--ml-model-id")] string MlModelId,
[property: CommandSwitch("--batch-prediction-data-source-id")] string BatchPredictionDataSourceId,
[property: CommandSwitch("--output-uri")] string OutputUri
) : AwsOptions
{
    [CommandSwitch("--batch-prediction-name")]
    public string? BatchPredictionName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}