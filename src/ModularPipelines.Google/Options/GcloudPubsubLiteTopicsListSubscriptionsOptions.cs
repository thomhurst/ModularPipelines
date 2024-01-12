using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-topics", "list-subscriptions")]
public record GcloudPubsubLiteTopicsListSubscriptionsOptions(
[property: PositionalArgument] string Topic,
[property: PositionalArgument] string Location
) : GcloudOptions;