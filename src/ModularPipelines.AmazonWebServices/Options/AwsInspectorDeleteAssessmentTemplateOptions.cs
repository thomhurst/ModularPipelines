using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "delete-assessment-template")]
public record AwsInspectorDeleteAssessmentTemplateOptions(
[property: CliOption("--assessment-template-arn")] string AssessmentTemplateArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}