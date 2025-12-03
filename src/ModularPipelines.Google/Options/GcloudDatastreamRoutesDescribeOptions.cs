using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "routes", "describe")]
public record GcloudDatastreamRoutesDescribeOptions(
[property: CliArgument] string Route,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateConnection
) : GcloudOptions;