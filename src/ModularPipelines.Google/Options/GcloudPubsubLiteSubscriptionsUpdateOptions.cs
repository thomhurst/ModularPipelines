using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-subscriptions", "update")]
public record GcloudPubsubLiteSubscriptionsUpdateOptions(
[property: CliArgument] string Subscription,
[property: CliArgument] string Location,
[property: CliOption("--delivery-requirement")] string DeliveryRequirement,
[property: CliOption("--export-dead-letter-topic")] string ExportDeadLetterTopic,
[property: CliOption("--export-desired-state")] string ExportDesiredState,
[property: CliOption("--export-pubsub-topic")] string ExportPubsubTopic
) : GcloudOptions;