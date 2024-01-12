using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "routes", "list")]
public record GcloudDatastreamRoutesListOptions(
[property: CommandSwitch("--private-connection")] string PrivateConnection,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;