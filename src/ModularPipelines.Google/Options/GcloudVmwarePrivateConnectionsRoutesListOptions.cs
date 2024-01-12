using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-connections", "routes", "list")]
public record GcloudVmwarePrivateConnectionsRoutesListOptions(
[property: CommandSwitch("--private-connection")] string PrivateConnection,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;