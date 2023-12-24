using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pi", "describe-dimension-keys")]
public record AwsPiDescribeDimensionKeysOptions(
[property: CommandSwitch("--service-type")] string ServiceType,
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--metric")] string Metric,
[property: CommandSwitch("--group-by")] string GroupBy
) : AwsOptions
{
    [CommandSwitch("--period-in-seconds")]
    public int? PeriodInSeconds { get; set; }

    [CommandSwitch("--additional-metrics")]
    public string[]? AdditionalMetrics { get; set; }

    [CommandSwitch("--partition-by")]
    public string? PartitionBy { get; set; }

    [CommandSwitch("--filter")]
    public IEnumerable<KeyValue>? Filter { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}