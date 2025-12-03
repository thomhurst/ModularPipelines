using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("import-export", "location", "list")]
public record AzImportExportLocationListOptions : AzOptions;