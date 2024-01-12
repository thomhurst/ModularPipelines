using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-subscriptions", "describe")]
public record GcloudPubsubLiteSubscriptionsDescribeOptions(
[property: PositionalArgument] string Subscription,
[property: PositionalArgument] string Location
) : GcloudOptions;