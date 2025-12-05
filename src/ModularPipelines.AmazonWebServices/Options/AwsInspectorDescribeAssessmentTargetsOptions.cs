using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "describe-assessment-targets")]
public record AwsInspectorDescribeAssessmentTargetsOptions(
[property: CliOption("--assessment-target-arns")] string[] AssessmentTargetArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}