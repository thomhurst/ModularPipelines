using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "operations", "list")]
public record GcloudDatastreamOperationsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;