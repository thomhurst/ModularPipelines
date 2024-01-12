using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "locations", "describe")]
public record GcloudDatastreamLocationsDescribeOptions(
[property: PositionalArgument] string Location
) : GcloudOptions;