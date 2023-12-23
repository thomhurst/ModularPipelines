using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "get-cost-forecast")]
public record AwsCeGetCostForecastOptions(
[property: CommandSwitch("--time-period")] string TimePeriod,
[property: CommandSwitch("--metric")] string Metric,
[property: CommandSwitch("--granularity")] string Granularity
) : AwsOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--prediction-interval-level")]
    public int? PredictionIntervalLevel { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}