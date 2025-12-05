using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "routes", "list")]
public record GcloudDatastreamRoutesListOptions(
[property: CliOption("--private-connection")] string PrivateConnection,
[property: CliOption("--location")] string Location
) : GcloudOptions;