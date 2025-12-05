using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "streams", "describe")]
public record GcloudDatastreamStreamsDescribeOptions(
[property: CliArgument] string Stream,
[property: CliArgument] string Location
) : GcloudOptions;