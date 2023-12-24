using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "delete-batch-prediction")]
public record AwsMachinelearningDeleteBatchPredictionOptions(
[property: CommandSwitch("--batch-prediction-id")] string BatchPredictionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}