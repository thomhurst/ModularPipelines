using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "taxonomies", "list")]
public record GcloudDataCatalogTaxonomiesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;