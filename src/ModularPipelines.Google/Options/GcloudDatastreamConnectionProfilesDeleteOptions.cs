using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "connection-profiles", "delete")]
public record GcloudDatastreamConnectionProfilesDeleteOptions(
[property: PositionalArgument] string ConnectionProfile,
[property: PositionalArgument] string Location
) : GcloudOptions;