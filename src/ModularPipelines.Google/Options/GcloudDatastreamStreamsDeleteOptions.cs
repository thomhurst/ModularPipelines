using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "streams", "delete")]
public record GcloudDatastreamStreamsDeleteOptions(
[property: CliArgument] string Stream,
[property: CliArgument] string Location
) : GcloudOptions;