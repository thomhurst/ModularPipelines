using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "delete-timeline-event")]
public record AwsSsmIncidentsDeleteTimelineEventOptions(
[property: CliOption("--event-id")] string EventId,
[property: CliOption("--incident-record-arn")] string IncidentRecordArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}