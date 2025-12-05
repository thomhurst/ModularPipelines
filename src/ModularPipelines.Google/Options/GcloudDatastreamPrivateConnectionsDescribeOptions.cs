using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "private-connections", "describe")]
public record GcloudDatastreamPrivateConnectionsDescribeOptions(
[property: CliArgument] string PrivateConnection,
[property: CliArgument] string Location
) : GcloudOptions;