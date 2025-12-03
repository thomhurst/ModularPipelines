using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("import-export", "location", "show")]
public record AzImportExportLocationShowOptions(
[property: CliOption("--location")] string Location
) : AzOptions;