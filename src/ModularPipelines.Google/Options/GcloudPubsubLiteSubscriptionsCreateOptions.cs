using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-subscriptions", "create")]
public record GcloudPubsubLiteSubscriptionsCreateOptions(
[property: PositionalArgument] string Subscription,
[property: CommandSwitch("--topic")] string Topic
) : GcloudOptions
{
    [CommandSwitch("--delivery-requirement")]
    public string? DeliveryRequirement { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--event-time")]
    public string? EventTime { get; set; }

    [CommandSwitch("--publish-time")]
    public string? PublishTime { get; set; }

    [CommandSwitch("--starting-offset")]
    public string? StartingOffset { get; set; }

    [CommandSwitch("--export-pubsub-topic")]
    public string? ExportPubsubTopic { get; set; }

    [CommandSwitch("--export-dead-letter-topic")]
    public string? ExportDeadLetterTopic { get; set; }

    [CommandSwitch("--export-desired-state")]
    public string? ExportDesiredState { get; set; }
}