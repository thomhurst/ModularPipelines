using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pi", "get-resource-metrics")]
public record AwsPiGetResourceMetricsOptions(
[property: CommandSwitch("--service-type")] string ServiceType,
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--metric-queries")] string[] MetricQueries,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime
) : AwsOptions
{
    [CommandSwitch("--period-in-seconds")]
    public int? PeriodInSeconds { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--period-alignment")]
    public string? PeriodAlignment { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}