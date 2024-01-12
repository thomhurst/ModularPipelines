using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "pull")]
public record GcloudPubsubSubscriptionsPullOptions(
[property: PositionalArgument] string Subscription
) : GcloudOptions
{
    [BooleanCommandSwitch("--auto-ack")]
    public bool? AutoAck { get; set; }
}