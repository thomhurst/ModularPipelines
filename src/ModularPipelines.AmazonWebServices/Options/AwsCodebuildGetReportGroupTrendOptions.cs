using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codebuild", "get-report-group-trend")]
public record AwsCodebuildGetReportGroupTrendOptions(
[property: CommandSwitch("--report-group-arn")] string ReportGroupArn,
[property: CommandSwitch("--trend-field")] string TrendField
) : AwsOptions
{
    [CommandSwitch("--num-of-reports")]
    public int? NumOfReports { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}