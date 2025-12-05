using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-subscriptions", "create")]
public record GcloudPubsubLiteSubscriptionsCreateOptions(
[property: CliArgument] string Subscription,
[property: CliOption("--topic")] string Topic
) : GcloudOptions
{
    [CliOption("--delivery-requirement")]
    public string? DeliveryRequirement { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--event-time")]
    public string? EventTime { get; set; }

    [CliOption("--publish-time")]
    public string? PublishTime { get; set; }

    [CliOption("--starting-offset")]
    public string? StartingOffset { get; set; }

    [CliOption("--export-pubsub-topic")]
    public string? ExportPubsubTopic { get; set; }

    [CliOption("--export-dead-letter-topic")]
    public string? ExportDeadLetterTopic { get; set; }

    [CliOption("--export-desired-state")]
    public string? ExportDesiredState { get; set; }
}