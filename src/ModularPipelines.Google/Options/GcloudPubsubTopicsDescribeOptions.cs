using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "topics", "describe")]
public record GcloudPubsubTopicsDescribeOptions(
[property: CliArgument] string Topic
) : GcloudOptions;