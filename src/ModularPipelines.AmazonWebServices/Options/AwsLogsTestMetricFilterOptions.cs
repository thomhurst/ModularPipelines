using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "test-metric-filter")]
public record AwsLogsTestMetricFilterOptions(
[property: CliOption("--filter-pattern")] string FilterPattern,
[property: CliOption("--log-event-messages")] string[] LogEventMessages
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}