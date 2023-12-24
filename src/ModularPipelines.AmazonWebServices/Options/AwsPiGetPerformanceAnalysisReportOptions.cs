using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pi", "get-performance-analysis-report")]
public record AwsPiGetPerformanceAnalysisReportOptions(
[property: CommandSwitch("--service-type")] string ServiceType,
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--analysis-report-id")] string AnalysisReportId
) : AwsOptions
{
    [CommandSwitch("--text-format")]
    public string? TextFormat { get; set; }

    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}