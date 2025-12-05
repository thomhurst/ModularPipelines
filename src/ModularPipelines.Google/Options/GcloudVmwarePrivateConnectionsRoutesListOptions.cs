using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-connections", "routes", "list")]
public record GcloudVmwarePrivateConnectionsRoutesListOptions(
[property: CliOption("--private-connection")] string PrivateConnection,
[property: CliOption("--location")] string Location
) : GcloudOptions;