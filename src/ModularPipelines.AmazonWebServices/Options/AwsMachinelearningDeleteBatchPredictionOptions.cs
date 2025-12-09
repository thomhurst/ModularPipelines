using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "delete-batch-prediction")]
public record AwsMachinelearningDeleteBatchPredictionOptions(
[property: CliOption("--batch-prediction-id")] string BatchPredictionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}