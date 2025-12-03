using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "create-assessment-template")]
public record AwsInspectorCreateAssessmentTemplateOptions(
[property: CliOption("--assessment-target-arn")] string AssessmentTargetArn,
[property: CliOption("--assessment-template-name")] string AssessmentTemplateName,
[property: CliOption("--duration-in-seconds")] int DurationInSeconds,
[property: CliOption("--rules-package-arns")] string[] RulesPackageArns
) : AwsOptions
{
    [CliOption("--user-attributes-for-findings")]
    public string[]? UserAttributesForFindings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}