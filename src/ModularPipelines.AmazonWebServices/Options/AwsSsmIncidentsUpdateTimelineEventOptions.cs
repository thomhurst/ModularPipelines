using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-incidents", "update-timeline-event")]
public record AwsSsmIncidentsUpdateTimelineEventOptions(
[property: CommandSwitch("--event-id")] string EventId,
[property: CommandSwitch("--incident-record-arn")] string IncidentRecordArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--event-data")]
    public string? EventData { get; set; }

    [CommandSwitch("--event-references")]
    public string[]? EventReferences { get; set; }

    [CommandSwitch("--event-time")]
    public long? EventTime { get; set; }

    [CommandSwitch("--event-type")]
    public string? EventType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}