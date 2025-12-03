using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "entry-groups", "list")]
public record GcloudDataCatalogEntryGroupsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;