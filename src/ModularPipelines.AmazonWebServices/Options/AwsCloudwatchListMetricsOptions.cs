using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "list-metrics")]
public record AwsCloudwatchListMetricsOptions : AwsOptions
{
    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--metric-name")]
    public string? MetricName { get; set; }

    [CommandSwitch("--dimensions")]
    public string[]? Dimensions { get; set; }

    [CommandSwitch("--recently-active")]
    public string? RecentlyActive { get; set; }

    [CommandSwitch("--owning-account")]
    public string? OwningAccount { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}