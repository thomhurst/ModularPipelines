using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "topics", "detach-subscription")]
public record GcloudPubsubTopicsDetachSubscriptionOptions(
[property: CliArgument] string Subscription
) : GcloudOptions;