using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "ack")]
public record GcloudPubsubSubscriptionsAckOptions(
[property: PositionalArgument] string Subscription,
[property: CommandSwitch("--ack-ids")] string[] AckIds
) : GcloudOptions;