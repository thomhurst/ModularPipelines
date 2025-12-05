using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-subscriptions", "delete")]
public record GcloudPubsubLiteSubscriptionsDeleteOptions(
[property: CliArgument] string Subscription,
[property: CliArgument] string Location
) : GcloudOptions;