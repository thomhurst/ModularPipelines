using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "describe-assessment-runs")]
public record AwsInspectorDescribeAssessmentRunsOptions(
[property: CommandSwitch("--assessment-run-arns")] string[] AssessmentRunArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}