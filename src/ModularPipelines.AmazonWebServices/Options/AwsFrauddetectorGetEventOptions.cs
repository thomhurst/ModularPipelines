using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "get-event")]
public record AwsFrauddetectorGetEventOptions(
[property: CommandSwitch("--event-id")] string EventId,
[property: CommandSwitch("--event-type-name")] string EventTypeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}