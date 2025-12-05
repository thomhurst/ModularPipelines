using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "get-current-metric-data")]
public record AwsConnectGetCurrentMetricDataOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--filters")] string Filters,
[property: CliOption("--current-metrics")] string[] CurrentMetrics
) : AwsOptions
{
    [CliOption("--groupings")]
    public string[]? Groupings { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--sort-criteria")]
    public string[]? SortCriteria { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}