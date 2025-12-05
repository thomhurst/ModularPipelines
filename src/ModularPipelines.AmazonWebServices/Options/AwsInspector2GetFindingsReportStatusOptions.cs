using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "get-findings-report-status")]
public record AwsInspector2GetFindingsReportStatusOptions : AwsOptions
{
    [CliOption("--report-id")]
    public string? ReportId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}