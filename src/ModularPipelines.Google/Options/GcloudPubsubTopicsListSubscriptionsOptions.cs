using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "topics", "list-subscriptions")]
public record GcloudPubsubTopicsListSubscriptionsOptions(
[property: PositionalArgument] string Topic
) : GcloudOptions;