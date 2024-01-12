using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "topics", "publish")]
public record GcloudPubsubTopicsPublishOptions(
[property: PositionalArgument] string Topic
) : GcloudOptions
{
    [CommandSwitch("--attribute")]
    public string[]? Attribute { get; set; }

    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--ordering-key")]
    public string? OrderingKey { get; set; }
}