using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-topics", "describe")]
public record GcloudPubsubLiteTopicsDescribeOptions(
[property: PositionalArgument] string Topic,
[property: PositionalArgument] string Location
) : GcloudOptions;