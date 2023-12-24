using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "cancel-batch-prediction-job")]
public record AwsFrauddetectorCancelBatchPredictionJobOptions(
[property: CommandSwitch("--job-id")] string JobId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}