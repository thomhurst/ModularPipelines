using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "describe-assessment-templates")]
public record AwsInspectorDescribeAssessmentTemplatesOptions(
[property: CliOption("--assessment-template-arns")] string[] AssessmentTemplateArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}