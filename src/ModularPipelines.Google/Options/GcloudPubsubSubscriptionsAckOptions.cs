using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "ack")]
public record GcloudPubsubSubscriptionsAckOptions(
[property: CliArgument] string Subscription,
[property: CliOption("--ack-ids")] string[] AckIds
) : GcloudOptions;