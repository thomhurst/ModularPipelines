using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "search")]
public record GcloudDataCatalogSearchOptions(
[property: CliArgument] string Query,
[property: CliFlag("--include-gcp-public-datasets")] bool IncludeGcpPublicDatasets,
[property: CliOption("--include-organization-ids")] string[] IncludeOrganizationIds,
[property: CliOption("--include-project-ids")] string[] IncludeProjectIds,
[property: CliOption("--restricted-locations")] string[] RestrictedLocations
) : GcloudOptions;