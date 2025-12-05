using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pi", "get-performance-analysis-report")]
public record AwsPiGetPerformanceAnalysisReportOptions(
[property: CliOption("--service-type")] string ServiceType,
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--analysis-report-id")] string AnalysisReportId
) : AwsOptions
{
    [CliOption("--text-format")]
    public string? TextFormat { get; set; }

    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}