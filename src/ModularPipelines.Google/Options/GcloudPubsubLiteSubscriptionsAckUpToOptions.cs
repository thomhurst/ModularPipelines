using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-subscriptions", "ack-up-to")]
public record GcloudPubsubLiteSubscriptionsAckUpToOptions(
[property: PositionalArgument] string Subscription,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--offset")] string Offset,
[property: CommandSwitch("--partition")] string Partition
) : GcloudOptions;