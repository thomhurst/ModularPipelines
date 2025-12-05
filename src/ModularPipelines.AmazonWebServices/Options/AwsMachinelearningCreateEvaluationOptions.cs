using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "create-evaluation")]
public record AwsMachinelearningCreateEvaluationOptions(
[property: CliOption("--evaluation-id")] string EvaluationId,
[property: CliOption("--ml-model-id")] string MlModelId,
[property: CliOption("--evaluation-data-source-id")] string EvaluationDataSourceId
) : AwsOptions
{
    [CliOption("--evaluation-name")]
    public string? EvaluationName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}