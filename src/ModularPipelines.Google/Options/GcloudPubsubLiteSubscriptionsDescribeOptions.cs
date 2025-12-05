using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-subscriptions", "describe")]
public record GcloudPubsubLiteSubscriptionsDescribeOptions(
[property: CliArgument] string Subscription,
[property: CliArgument] string Location
) : GcloudOptions;