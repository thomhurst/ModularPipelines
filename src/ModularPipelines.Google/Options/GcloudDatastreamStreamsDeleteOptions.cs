using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "streams", "delete")]
public record GcloudDatastreamStreamsDeleteOptions(
[property: PositionalArgument] string Stream,
[property: PositionalArgument] string Location
) : GcloudOptions;