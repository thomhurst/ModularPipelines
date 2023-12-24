using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhubstrategy", "start-assessment")]
public record AwsMigrationhubstrategyStartAssessmentOptions : AwsOptions
{
    [CommandSwitch("--assessment-data-source-type")]
    public string? AssessmentDataSourceType { get; set; }

    [CommandSwitch("--assessment-targets")]
    public string[]? AssessmentTargets { get; set; }

    [CommandSwitch("--s3bucket-for-analysis-data")]
    public string? S3bucketForAnalysisData { get; set; }

    [CommandSwitch("--s3bucket-for-report-data")]
    public string? S3bucketForReportData { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}