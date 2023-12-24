using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "get-batch-prediction")]
public record AwsMachinelearningGetBatchPredictionOptions(
[property: CommandSwitch("--batch-prediction-id")] string BatchPredictionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}