using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-subscriptions", "list")]
public record GcloudPubsubLiteSubscriptionsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}