using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-metric-filter")]
public record AwsLogsPutMetricFilterOptions(
[property: CliOption("--log-group-name")] string LogGroupName,
[property: CliOption("--filter-name")] string FilterName,
[property: CliOption("--filter-pattern")] string FilterPattern,
[property: CliOption("--metric-transformations")] string[] MetricTransformations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}