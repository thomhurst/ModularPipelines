using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "send-event")]
public record AwsFrauddetectorSendEventOptions(
[property: CommandSwitch("--event-id")] string EventId,
[property: CommandSwitch("--event-type-name")] string EventTypeName,
[property: CommandSwitch("--event-timestamp")] string EventTimestamp,
[property: CommandSwitch("--event-variables")] IEnumerable<KeyValue> EventVariables,
[property: CommandSwitch("--entities")] string[] Entities
) : AwsOptions
{
    [CommandSwitch("--assigned-label")]
    public string? AssignedLabel { get; set; }

    [CommandSwitch("--label-timestamp")]
    public string? LabelTimestamp { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}