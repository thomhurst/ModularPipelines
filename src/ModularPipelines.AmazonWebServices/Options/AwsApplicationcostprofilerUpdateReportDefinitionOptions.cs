using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("applicationcostprofiler", "update-report-definition")]
public record AwsApplicationcostprofilerUpdateReportDefinitionOptions(
[property: CliOption("--report-id")] string ReportId,
[property: CliOption("--report-description")] string ReportDescription,
[property: CliOption("--report-frequency")] string ReportFrequency,
[property: CliOption("--format")] string Format,
[property: CliOption("--destination-s3-location")] string DestinationS3Location
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}