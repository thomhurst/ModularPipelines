using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-topics", "delete")]
public record GcloudPubsubLiteTopicsDeleteOptions(
[property: PositionalArgument] string Topic,
[property: PositionalArgument] string Location
) : GcloudOptions;