using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "zones", "list")]
public record GcloudDataplexZonesListOptions(
[property: CliOption("--lake")] string Lake,
[property: CliOption("--location")] string Location
) : GcloudOptions;