using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-incidents", "delete-timeline-event")]
public record AwsSsmIncidentsDeleteTimelineEventOptions(
[property: CommandSwitch("--event-id")] string EventId,
[property: CommandSwitch("--incident-record-arn")] string IncidentRecordArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}