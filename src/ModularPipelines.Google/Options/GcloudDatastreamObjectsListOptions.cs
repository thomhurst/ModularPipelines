using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "objects", "list")]
public record GcloudDatastreamObjectsListOptions(
[property: CliOption("--stream")] string Stream,
[property: CliOption("--location")] string Location
) : GcloudOptions;