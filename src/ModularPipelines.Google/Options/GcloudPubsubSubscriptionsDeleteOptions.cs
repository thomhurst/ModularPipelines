using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "delete")]
public record GcloudPubsubSubscriptionsDeleteOptions(
[property: CliArgument] string Subscription
) : GcloudOptions;