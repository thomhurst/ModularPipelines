using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "update-timeline-event")]
public record AwsSsmIncidentsUpdateTimelineEventOptions(
[property: CliOption("--event-id")] string EventId,
[property: CliOption("--incident-record-arn")] string IncidentRecordArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--event-data")]
    public string? EventData { get; set; }

    [CliOption("--event-references")]
    public string[]? EventReferences { get; set; }

    [CliOption("--event-time")]
    public long? EventTime { get; set; }

    [CliOption("--event-type")]
    public string? EventType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}