using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "create-assessment-target")]
public record AwsInspectorCreateAssessmentTargetOptions(
[property: CommandSwitch("--assessment-target-name")] string AssessmentTargetName
) : AwsOptions
{
    [CommandSwitch("--resource-group-arn")]
    public string? ResourceGroupArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}