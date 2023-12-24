using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "get-assessment-report")]
public record AwsInspectorGetAssessmentReportOptions(
[property: CommandSwitch("--assessment-run-arn")] string AssessmentRunArn,
[property: CommandSwitch("--report-file-format")] string ReportFileFormat,
[property: CommandSwitch("--report-type")] string ReportType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}