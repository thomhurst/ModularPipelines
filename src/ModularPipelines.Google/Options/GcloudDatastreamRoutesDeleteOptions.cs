using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "routes", "delete")]
public record GcloudDatastreamRoutesDeleteOptions(
[property: CliArgument] string Route,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateConnection
) : GcloudOptions;