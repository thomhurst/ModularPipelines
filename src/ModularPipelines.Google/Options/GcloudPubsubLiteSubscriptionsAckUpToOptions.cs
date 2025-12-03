using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-subscriptions", "ack-up-to")]
public record GcloudPubsubLiteSubscriptionsAckUpToOptions(
[property: CliArgument] string Subscription,
[property: CliArgument] string Location,
[property: CliOption("--offset")] string Offset,
[property: CliOption("--partition")] string Partition
) : GcloudOptions;