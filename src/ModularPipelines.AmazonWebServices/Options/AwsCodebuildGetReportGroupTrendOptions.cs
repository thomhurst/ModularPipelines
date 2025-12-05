using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "get-report-group-trend")]
public record AwsCodebuildGetReportGroupTrendOptions(
[property: CliOption("--report-group-arn")] string ReportGroupArn,
[property: CliOption("--trend-field")] string TrendField
) : AwsOptions
{
    [CliOption("--num-of-reports")]
    public int? NumOfReports { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}