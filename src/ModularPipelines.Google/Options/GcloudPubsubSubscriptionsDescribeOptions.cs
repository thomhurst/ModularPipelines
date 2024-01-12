using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "describe")]
public record GcloudPubsubSubscriptionsDescribeOptions(
[property: PositionalArgument] string Subscription
) : GcloudOptions;