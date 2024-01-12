using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-topics", "publish")]
public record GcloudPubsubLiteTopicsPublishOptions(
[property: PositionalArgument] string Topic,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--event-time")]
    public string? EventTime { get; set; }

    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--ordering-key")]
    public string? OrderingKey { get; set; }
}