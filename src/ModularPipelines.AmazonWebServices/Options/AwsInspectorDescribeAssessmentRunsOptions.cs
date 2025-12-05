using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "describe-assessment-runs")]
public record AwsInspectorDescribeAssessmentRunsOptions(
[property: CliOption("--assessment-run-arns")] string[] AssessmentRunArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}