using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "predict")]
public record AwsMachinelearningPredictOptions(
[property: CliOption("--ml-model-id")] string MlModelId,
[property: CliOption("--record")] IEnumerable<KeyValue> Record,
[property: CliOption("--predict-endpoint")] string PredictEndpoint
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}