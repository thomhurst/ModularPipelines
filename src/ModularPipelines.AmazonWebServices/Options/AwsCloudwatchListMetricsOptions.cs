using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "list-metrics")]
public record AwsCloudwatchListMetricsOptions : AwsOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--metric-name")]
    public string? MetricName { get; set; }

    [CliOption("--dimensions")]
    public string[]? Dimensions { get; set; }

    [CliOption("--recently-active")]
    public string? RecentlyActive { get; set; }

    [CliOption("--owning-account")]
    public string? OwningAccount { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}