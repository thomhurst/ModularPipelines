using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "connection-profiles", "describe")]
public record GcloudDatastreamConnectionProfilesDescribeOptions(
[property: CliArgument] string ConnectionProfile,
[property: CliArgument] string Location
) : GcloudOptions;