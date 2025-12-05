using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "update-evaluation")]
public record AwsMachinelearningUpdateEvaluationOptions(
[property: CliOption("--evaluation-id")] string EvaluationId,
[property: CliOption("--evaluation-name")] string EvaluationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}