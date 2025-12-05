using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pi", "delete-performance-analysis-report")]
public record AwsPiDeletePerformanceAnalysisReportOptions(
[property: CliOption("--service-type")] string ServiceType,
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--analysis-report-id")] string AnalysisReportId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}