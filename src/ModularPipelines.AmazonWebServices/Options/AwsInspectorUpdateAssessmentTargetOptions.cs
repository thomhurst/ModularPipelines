using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "update-assessment-target")]
public record AwsInspectorUpdateAssessmentTargetOptions(
[property: CliOption("--assessment-target-arn")] string AssessmentTargetArn,
[property: CliOption("--assessment-target-name")] string AssessmentTargetName
) : AwsOptions
{
    [CliOption("--resource-group-arn")]
    public string? ResourceGroupArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}