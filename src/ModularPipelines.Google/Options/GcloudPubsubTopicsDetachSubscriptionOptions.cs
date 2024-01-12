using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "topics", "detach-subscription")]
public record GcloudPubsubTopicsDetachSubscriptionOptions(
[property: PositionalArgument] string Subscription
) : GcloudOptions;