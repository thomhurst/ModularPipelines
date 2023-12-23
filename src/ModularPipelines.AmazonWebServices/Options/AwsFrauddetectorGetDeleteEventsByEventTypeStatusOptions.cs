using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "get-delete-events-by-event-type-status")]
public record AwsFrauddetectorGetDeleteEventsByEventTypeStatusOptions(
[property: CommandSwitch("--event-type-name")] string EventTypeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}