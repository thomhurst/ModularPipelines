using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "modify-message-ack-deadline")]
public record GcloudPubsubSubscriptionsModifyMessageAckDeadlineOptions(
[property: PositionalArgument] string Subscription,
[property: CommandSwitch("--ack-deadline")] string AckDeadline,
[property: CommandSwitch("--ack-ids")] string[] AckIds
) : GcloudOptions;