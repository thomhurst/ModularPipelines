using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "test-event-pattern")]
public record AwsEventsTestEventPatternOptions(
[property: CliOption("--event-pattern")] string EventPattern,
[property: CliOption("--event")] string Event
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}