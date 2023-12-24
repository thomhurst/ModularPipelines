using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhubstrategy", "start-recommendation-report-generation")]
public record AwsMigrationhubstrategyStartRecommendationReportGenerationOptions : AwsOptions
{
    [CommandSwitch("--group-id-filter")]
    public string[]? GroupIdFilter { get; set; }

    [CommandSwitch("--output-format")]
    public string? OutputFormat { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}