using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "update-evaluation")]
public record AwsMachinelearningUpdateEvaluationOptions(
[property: CommandSwitch("--evaluation-id")] string EvaluationId,
[property: CommandSwitch("--evaluation-name")] string EvaluationName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}