using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "taxonomies", "import")]
public record GcloudDataCatalogTaxonomiesImportOptions(
[property: PositionalArgument] string Taxonomies,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;