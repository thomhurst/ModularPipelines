using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "routes", "describe")]
public record GcloudDatastreamRoutesDescribeOptions(
[property: PositionalArgument] string Route,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateConnection
) : GcloudOptions;