using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "update-event-label")]
public record AwsFrauddetectorUpdateEventLabelOptions(
[property: CommandSwitch("--event-id")] string EventId,
[property: CommandSwitch("--event-type-name")] string EventTypeName,
[property: CommandSwitch("--assigned-label")] string AssignedLabel,
[property: CommandSwitch("--label-timestamp")] string LabelTimestamp
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}