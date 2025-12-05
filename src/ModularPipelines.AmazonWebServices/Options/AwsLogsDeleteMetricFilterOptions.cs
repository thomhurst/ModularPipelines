using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "delete-metric-filter")]
public record AwsLogsDeleteMetricFilterOptions(
[property: CliOption("--log-group-name")] string LogGroupName,
[property: CliOption("--filter-name")] string FilterName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}