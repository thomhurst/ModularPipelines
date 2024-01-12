using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-subscriptions", "update")]
public record GcloudPubsubLiteSubscriptionsUpdateOptions(
[property: PositionalArgument] string Subscription,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--delivery-requirement")] string DeliveryRequirement,
[property: CommandSwitch("--export-dead-letter-topic")] string ExportDeadLetterTopic,
[property: CommandSwitch("--export-desired-state")] string ExportDesiredState,
[property: CommandSwitch("--export-pubsub-topic")] string ExportPubsubTopic
) : GcloudOptions;