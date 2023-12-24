using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "describe-assessment-templates")]
public record AwsInspectorDescribeAssessmentTemplatesOptions(
[property: CommandSwitch("--assessment-template-arns")] string[] AssessmentTemplateArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}