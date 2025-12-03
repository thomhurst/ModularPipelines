using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-connections", "describe")]
public record GcloudVmwarePrivateConnectionsDescribeOptions(
[property: CliArgument] string PrivateConnection,
[property: CliArgument] string Location
) : GcloudOptions;