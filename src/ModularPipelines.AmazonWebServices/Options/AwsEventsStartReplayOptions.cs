using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "start-replay")]
public record AwsEventsStartReplayOptions(
[property: CliOption("--replay-name")] string ReplayName,
[property: CliOption("--event-source-arn")] string EventSourceArn,
[property: CliOption("--event-start-time")] long EventStartTime,
[property: CliOption("--event-end-time")] long EventEndTime,
[property: CliOption("--destination")] string Destination
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}