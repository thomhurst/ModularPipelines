using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "describe")]
public record GcloudPubsubSubscriptionsDescribeOptions(
[property: CliArgument] string Subscription
) : GcloudOptions;