using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "entries", "lookup")]
public record GcloudDataCatalogEntriesLookupOptions(
[property: CliArgument] string Resource
) : GcloudOptions;