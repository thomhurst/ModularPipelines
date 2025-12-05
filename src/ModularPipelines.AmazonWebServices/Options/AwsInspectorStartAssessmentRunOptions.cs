using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "start-assessment-run")]
public record AwsInspectorStartAssessmentRunOptions(
[property: CliOption("--assessment-template-arn")] string AssessmentTemplateArn
) : AwsOptions
{
    [CliOption("--assessment-run-name")]
    public string? AssessmentRunName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}