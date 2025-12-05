using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "get-assessment-report")]
public record AwsInspectorGetAssessmentReportOptions(
[property: CliOption("--assessment-run-arn")] string AssessmentRunArn,
[property: CliOption("--report-file-format")] string ReportFileFormat,
[property: CliOption("--report-type")] string ReportType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}