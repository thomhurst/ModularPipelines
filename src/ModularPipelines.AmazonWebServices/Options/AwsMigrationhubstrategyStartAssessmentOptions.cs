using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhubstrategy", "start-assessment")]
public record AwsMigrationhubstrategyStartAssessmentOptions : AwsOptions
{
    [CliOption("--assessment-data-source-type")]
    public string? AssessmentDataSourceType { get; set; }

    [CliOption("--assessment-targets")]
    public string[]? AssessmentTargets { get; set; }

    [CliOption("--s3bucket-for-analysis-data")]
    public string? S3bucketForAnalysisData { get; set; }

    [CliOption("--s3bucket-for-report-data")]
    public string? S3bucketForReportData { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}