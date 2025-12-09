using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "create-sbom-export")]
public record AwsInspector2CreateSbomExportOptions(
[property: CliOption("--report-format")] string ReportFormat,
[property: CliOption("--s3-destination")] string S3Destination
) : AwsOptions
{
    [CliOption("--resource-filter-criteria")]
    public string? ResourceFilterCriteria { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}