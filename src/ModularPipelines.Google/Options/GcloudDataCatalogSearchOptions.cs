using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "search")]
public record GcloudDataCatalogSearchOptions(
[property: PositionalArgument] string Query,
[property: BooleanCommandSwitch("--include-gcp-public-datasets")] bool IncludeGcpPublicDatasets,
[property: CommandSwitch("--include-organization-ids")] string[] IncludeOrganizationIds,
[property: CommandSwitch("--include-project-ids")] string[] IncludeProjectIds,
[property: CommandSwitch("--restricted-locations")] string[] RestrictedLocations
) : GcloudOptions;