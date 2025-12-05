using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "topics", "delete")]
public record GcloudPubsubTopicsDeleteOptions(
[property: CliArgument] string Topic
) : GcloudOptions;