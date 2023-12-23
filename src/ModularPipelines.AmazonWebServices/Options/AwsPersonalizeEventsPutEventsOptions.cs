using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize-events", "put-events")]
public record AwsPersonalizeEventsPutEventsOptions(
[property: CommandSwitch("--tracking-id")] string TrackingId,
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--event-list")] string[] EventList
) : AwsOptions
{
    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}