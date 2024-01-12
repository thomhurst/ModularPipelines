using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "topics", "describe")]
public record GcloudPubsubTopicsDescribeOptions(
[property: PositionalArgument] string Topic
) : GcloudOptions;