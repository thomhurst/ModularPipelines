using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "stop-assessment-run")]
public record AwsInspectorStopAssessmentRunOptions(
[property: CliOption("--assessment-run-arn")] string AssessmentRunArn
) : AwsOptions
{
    [CliOption("--stop-action")]
    public string? StopAction { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}