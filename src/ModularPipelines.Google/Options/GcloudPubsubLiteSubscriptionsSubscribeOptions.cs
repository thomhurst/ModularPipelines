using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-subscriptions", "subscribe")]
public record GcloudPubsubLiteSubscriptionsSubscribeOptions(
[property: CliArgument] string Subscription,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--auto-ack")]
    public bool? AutoAck { get; set; }

    [CliOption("--num-messages")]
    public string? NumMessages { get; set; }

    [CliOption("--partitions")]
    public string[]? Partitions { get; set; }
}