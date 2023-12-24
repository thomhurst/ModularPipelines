using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-incidents", "get-timeline-event")]
public record AwsSsmIncidentsGetTimelineEventOptions(
[property: CommandSwitch("--event-id")] string EventId,
[property: CommandSwitch("--incident-record-arn")] string IncidentRecordArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}