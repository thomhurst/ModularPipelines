using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "describe-assessment-targets")]
public record AwsInspectorDescribeAssessmentTargetsOptions(
[property: CommandSwitch("--assessment-target-arns")] string[] AssessmentTargetArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}