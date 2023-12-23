using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "start-assessment-run")]
public record AwsInspectorStartAssessmentRunOptions(
[property: CommandSwitch("--assessment-template-arn")] string AssessmentTemplateArn
) : AwsOptions
{
    [CommandSwitch("--assessment-run-name")]
    public string? AssessmentRunName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}