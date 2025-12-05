using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-topics", "list-subscriptions")]
public record GcloudPubsubLiteTopicsListSubscriptionsOptions(
[property: CliArgument] string Topic,
[property: CliArgument] string Location
) : GcloudOptions;