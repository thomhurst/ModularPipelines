using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-topics", "publish")]
public record GcloudPubsubLiteTopicsPublishOptions(
[property: CliArgument] string Topic,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--event-time")]
    public string? EventTime { get; set; }

    [CliOption("--message")]
    public string? Message { get; set; }

    [CliOption("--ordering-key")]
    public string? OrderingKey { get; set; }
}