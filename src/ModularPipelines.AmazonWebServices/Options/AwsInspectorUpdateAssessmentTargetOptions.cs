using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "update-assessment-target")]
public record AwsInspectorUpdateAssessmentTargetOptions(
[property: CommandSwitch("--assessment-target-arn")] string AssessmentTargetArn,
[property: CommandSwitch("--assessment-target-name")] string AssessmentTargetName
) : AwsOptions
{
    [CommandSwitch("--resource-group-arn")]
    public string? ResourceGroupArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}