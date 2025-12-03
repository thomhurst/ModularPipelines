using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "delete-assessment-target")]
public record AwsInspectorDeleteAssessmentTargetOptions(
[property: CliOption("--assessment-target-arn")] string AssessmentTargetArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}