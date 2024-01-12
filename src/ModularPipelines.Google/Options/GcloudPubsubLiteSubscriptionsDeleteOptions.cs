using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-subscriptions", "delete")]
public record GcloudPubsubLiteSubscriptionsDeleteOptions(
[property: PositionalArgument] string Subscription,
[property: PositionalArgument] string Location
) : GcloudOptions;