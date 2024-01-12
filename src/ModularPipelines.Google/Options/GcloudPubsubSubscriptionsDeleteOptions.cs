using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "delete")]
public record GcloudPubsubSubscriptionsDeleteOptions(
[property: PositionalArgument] string Subscription
) : GcloudOptions;