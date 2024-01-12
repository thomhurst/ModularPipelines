using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "connection-profiles", "describe")]
public record GcloudDatastreamConnectionProfilesDescribeOptions(
[property: PositionalArgument] string ConnectionProfile,
[property: PositionalArgument] string Location
) : GcloudOptions;