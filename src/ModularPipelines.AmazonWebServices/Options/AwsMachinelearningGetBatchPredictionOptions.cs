using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "get-batch-prediction")]
public record AwsMachinelearningGetBatchPredictionOptions(
[property: CliOption("--batch-prediction-id")] string BatchPredictionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}