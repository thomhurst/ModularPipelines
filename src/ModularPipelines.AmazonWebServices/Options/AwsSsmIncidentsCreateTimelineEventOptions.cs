using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-incidents", "create-timeline-event")]
public record AwsSsmIncidentsCreateTimelineEventOptions(
[property: CommandSwitch("--event-data")] string EventData,
[property: CommandSwitch("--event-time")] long EventTime,
[property: CommandSwitch("--event-type")] string EventType,
[property: CommandSwitch("--incident-record-arn")] string IncidentRecordArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--event-references")]
    public string[]? EventReferences { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}