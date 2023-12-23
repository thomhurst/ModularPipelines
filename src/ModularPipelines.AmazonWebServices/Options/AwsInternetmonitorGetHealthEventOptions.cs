using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("internetmonitor", "get-health-event")]
public record AwsInternetmonitorGetHealthEventOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--event-id")] string EventId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}