using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "private-connections", "list")]
public record GcloudDatastreamPrivateConnectionsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;