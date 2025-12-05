using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "datascans", "jobs", "list")]
public record GcloudDataplexDatascansJobsListOptions(
[property: CliOption("--datascan")] string Datascan,
[property: CliOption("--location")] string Location
) : GcloudOptions;