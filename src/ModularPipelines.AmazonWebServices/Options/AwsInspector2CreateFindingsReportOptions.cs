using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "create-findings-report")]
public record AwsInspector2CreateFindingsReportOptions(
[property: CliOption("--report-format")] string ReportFormat,
[property: CliOption("--s3-destination")] string S3Destination
) : AwsOptions
{
    [CliOption("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}