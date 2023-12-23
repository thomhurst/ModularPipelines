using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "update-batch-prediction")]
public record AwsMachinelearningUpdateBatchPredictionOptions(
[property: CommandSwitch("--batch-prediction-id")] string BatchPredictionId,
[property: CommandSwitch("--batch-prediction-name")] string BatchPredictionName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}