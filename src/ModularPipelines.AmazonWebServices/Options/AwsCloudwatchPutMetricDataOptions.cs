using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "put-metric-data")]
public record AwsCloudwatchPutMetricDataOptions(
[property: CommandSwitch("--namespace")] string Namespace
) : AwsOptions
{
    [CommandSwitch("--metric-data")]
    public string[]? MetricData { get; set; }

    [CommandSwitch("--metric-name")]
    public string? MetricName { get; set; }

    [CommandSwitch("--timestamp")]
    public string? Timestamp { get; set; }

    [CommandSwitch("--unit")]
    public string? Unit { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }

    [CommandSwitch("--dimensions")]
    public string? Dimensions { get; set; }

    [CommandSwitch("--statistic-values")]
    public string? StatisticValues { get; set; }

    [CommandSwitch("--storage-resolution")]
    public string? StorageResolution { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}