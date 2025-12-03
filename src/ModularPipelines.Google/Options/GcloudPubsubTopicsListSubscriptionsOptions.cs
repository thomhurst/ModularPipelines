using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "topics", "list-subscriptions")]
public record GcloudPubsubTopicsListSubscriptionsOptions(
[property: CliArgument] string Topic
) : GcloudOptions;