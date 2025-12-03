using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "connection-profiles", "list")]
public record GcloudDatastreamConnectionProfilesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;