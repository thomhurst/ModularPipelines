using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "locations", "describe")]
public record GcloudDatastreamLocationsDescribeOptions(
[property: CliArgument] string Location
) : GcloudOptions;