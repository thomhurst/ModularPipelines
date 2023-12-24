using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "predict")]
public record AwsMachinelearningPredictOptions(
[property: CommandSwitch("--ml-model-id")] string MlModelId,
[property: CommandSwitch("--record")] IEnumerable<KeyValue> Record,
[property: CommandSwitch("--predict-endpoint")] string PredictEndpoint
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}