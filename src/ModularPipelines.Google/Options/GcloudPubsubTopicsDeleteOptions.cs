using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "topics", "delete")]
public record GcloudPubsubTopicsDeleteOptions(
[property: PositionalArgument] string Topic
) : GcloudOptions;