using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "private-connections", "describe")]
public record GcloudDatastreamPrivateConnectionsDescribeOptions(
[property: PositionalArgument] string PrivateConnection,
[property: PositionalArgument] string Location
) : GcloudOptions;