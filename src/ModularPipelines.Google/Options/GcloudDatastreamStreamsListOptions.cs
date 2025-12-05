using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "streams", "list")]
public record GcloudDatastreamStreamsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;