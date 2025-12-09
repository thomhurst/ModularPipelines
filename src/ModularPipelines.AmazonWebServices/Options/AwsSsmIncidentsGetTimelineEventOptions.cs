using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "get-timeline-event")]
public record AwsSsmIncidentsGetTimelineEventOptions(
[property: CliOption("--event-id")] string EventId,
[property: CliOption("--incident-record-arn")] string IncidentRecordArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}