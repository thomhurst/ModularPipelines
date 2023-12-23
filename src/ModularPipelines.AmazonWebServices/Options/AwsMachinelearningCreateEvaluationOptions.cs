using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "create-evaluation")]
public record AwsMachinelearningCreateEvaluationOptions(
[property: CommandSwitch("--evaluation-id")] string EvaluationId,
[property: CommandSwitch("--ml-model-id")] string MlModelId,
[property: CommandSwitch("--evaluation-data-source-id")] string EvaluationDataSourceId
) : AwsOptions
{
    [CommandSwitch("--evaluation-name")]
    public string? EvaluationName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}