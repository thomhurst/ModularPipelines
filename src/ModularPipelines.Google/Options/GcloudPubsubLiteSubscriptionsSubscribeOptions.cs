using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-subscriptions", "subscribe")]
public record GcloudPubsubLiteSubscriptionsSubscribeOptions(
[property: PositionalArgument] string Subscription,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--auto-ack")]
    public bool? AutoAck { get; set; }

    [CommandSwitch("--num-messages")]
    public string? NumMessages { get; set; }

    [CommandSwitch("--partitions")]
    public string[]? Partitions { get; set; }
}