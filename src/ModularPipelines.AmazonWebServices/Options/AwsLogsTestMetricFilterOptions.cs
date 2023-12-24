using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "test-metric-filter")]
public record AwsLogsTestMetricFilterOptions(
[property: CommandSwitch("--filter-pattern")] string FilterPattern,
[property: CommandSwitch("--log-event-messages")] string[] LogEventMessages
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}