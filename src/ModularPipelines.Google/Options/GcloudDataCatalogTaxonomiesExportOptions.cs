using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "taxonomies", "export")]
public record GcloudDataCatalogTaxonomiesExportOptions(
[property: PositionalArgument] string Taxonomies,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;