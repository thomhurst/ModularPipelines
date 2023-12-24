using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "test-event-pattern")]
public record AwsEventsTestEventPatternOptions(
[property: CommandSwitch("--event-pattern")] string EventPattern,
[property: CommandSwitch("--event")] string Event
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}