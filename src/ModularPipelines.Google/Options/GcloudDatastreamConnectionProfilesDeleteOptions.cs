using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "connection-profiles", "delete")]
public record GcloudDatastreamConnectionProfilesDeleteOptions(
[property: CliArgument] string ConnectionProfile,
[property: CliArgument] string Location
) : GcloudOptions;