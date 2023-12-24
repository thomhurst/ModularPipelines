using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "delete-event")]
public record AwsFrauddetectorDeleteEventOptions(
[property: CommandSwitch("--event-id")] string EventId,
[property: CommandSwitch("--event-type-name")] string EventTypeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}