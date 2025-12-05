using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "private-connections", "list")]
public record GcloudDatastreamPrivateConnectionsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;