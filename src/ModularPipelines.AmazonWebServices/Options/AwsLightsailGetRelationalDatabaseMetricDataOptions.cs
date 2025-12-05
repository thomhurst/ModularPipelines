using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-relational-database-metric-data")]
public record AwsLightsailGetRelationalDatabaseMetricDataOptions(
[property: CliOption("--relational-database-name")] string RelationalDatabaseName,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--period")] int Period,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--unit")] string Unit,
[property: CliOption("--statistics")] string[] Statistics
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}