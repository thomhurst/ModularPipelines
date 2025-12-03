using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "modify-message-ack-deadline")]
public record GcloudPubsubSubscriptionsModifyMessageAckDeadlineOptions(
[property: CliArgument] string Subscription,
[property: CliOption("--ack-deadline")] string AckDeadline,
[property: CliOption("--ack-ids")] string[] AckIds
) : GcloudOptions;