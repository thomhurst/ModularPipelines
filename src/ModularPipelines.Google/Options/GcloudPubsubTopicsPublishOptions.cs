using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "topics", "publish")]
public record GcloudPubsubTopicsPublishOptions(
[property: CliArgument] string Topic
) : GcloudOptions
{
    [CliOption("--attribute")]
    public string[]? Attribute { get; set; }

    [CliOption("--message")]
    public string? Message { get; set; }

    [CliOption("--ordering-key")]
    public string? OrderingKey { get; set; }
}