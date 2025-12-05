using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhubstrategy", "start-recommendation-report-generation")]
public record AwsMigrationhubstrategyStartRecommendationReportGenerationOptions : AwsOptions
{
    [CliOption("--group-id-filter")]
    public string[]? GroupIdFilter { get; set; }

    [CliOption("--output-format")]
    public string? OutputFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}