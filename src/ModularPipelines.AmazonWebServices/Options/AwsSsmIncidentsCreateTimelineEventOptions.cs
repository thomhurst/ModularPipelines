using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "create-timeline-event")]
public record AwsSsmIncidentsCreateTimelineEventOptions(
[property: CliOption("--event-data")] string EventData,
[property: CliOption("--event-time")] long EventTime,
[property: CliOption("--event-type")] string EventType,
[property: CliOption("--incident-record-arn")] string IncidentRecordArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--event-references")]
    public string[]? EventReferences { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}