using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "pull")]
public record GcloudPubsubSubscriptionsPullOptions(
[property: CliArgument] string Subscription
) : GcloudOptions
{
    [CliFlag("--auto-ack")]
    public bool? AutoAck { get; set; }
}