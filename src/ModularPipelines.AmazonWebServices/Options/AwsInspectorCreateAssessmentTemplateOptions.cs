using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "create-assessment-template")]
public record AwsInspectorCreateAssessmentTemplateOptions(
[property: CommandSwitch("--assessment-target-arn")] string AssessmentTargetArn,
[property: CommandSwitch("--assessment-template-name")] string AssessmentTemplateName,
[property: CommandSwitch("--duration-in-seconds")] int DurationInSeconds,
[property: CommandSwitch("--rules-package-arns")] string[] RulesPackageArns
) : AwsOptions
{
    [CommandSwitch("--user-attributes-for-findings")]
    public string[]? UserAttributesForFindings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}