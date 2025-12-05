using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "batch-get-report-groups")]
public record AwsCodebuildBatchGetReportGroupsOptions(
[property: CliOption("--report-group-arns")] string[] ReportGroupArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}