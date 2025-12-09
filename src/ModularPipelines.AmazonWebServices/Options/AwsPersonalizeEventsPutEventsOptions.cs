using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize-events", "put-events")]
public record AwsPersonalizeEventsPutEventsOptions(
[property: CliOption("--tracking-id")] string TrackingId,
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--event-list")] string[] EventList
) : AwsOptions
{
    [CliOption("--user-id")]
    public string? UserId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}