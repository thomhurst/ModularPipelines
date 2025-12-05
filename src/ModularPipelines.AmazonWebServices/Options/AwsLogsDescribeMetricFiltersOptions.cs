using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "describe-metric-filters")]
public record AwsLogsDescribeMetricFiltersOptions : AwsOptions
{
    [CliOption("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CliOption("--filter-name-prefix")]
    public string? FilterNamePrefix { get; set; }

    [CliOption("--metric-name")]
    public string? MetricName { get; set; }

    [CliOption("--metric-namespace")]
    public string? MetricNamespace { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}