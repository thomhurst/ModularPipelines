using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rum", "put-rum-events")]
public record AwsRumPutRumEventsOptions(
[property: CommandSwitch("--app-monitor-details")] string AppMonitorDetails,
[property: CommandSwitch("--batch-id")] string BatchId,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--rum-events")] string[] RumEvents,
[property: CommandSwitch("--user-details")] string UserDetails
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}