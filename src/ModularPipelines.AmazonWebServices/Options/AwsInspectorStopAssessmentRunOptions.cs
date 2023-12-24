using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "stop-assessment-run")]
public record AwsInspectorStopAssessmentRunOptions(
[property: CommandSwitch("--assessment-run-arn")] string AssessmentRunArn
) : AwsOptions
{
    [CommandSwitch("--stop-action")]
    public string? StopAction { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}