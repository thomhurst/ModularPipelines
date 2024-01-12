using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "connection-profiles", "list")]
public record GcloudDatastreamConnectionProfilesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;