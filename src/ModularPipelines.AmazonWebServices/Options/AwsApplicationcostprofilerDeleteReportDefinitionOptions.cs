using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("applicationcostprofiler", "delete-report-definition")]
public record AwsApplicationcostprofilerDeleteReportDefinitionOptions(
[property: CliOption("--report-id")] string ReportId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}