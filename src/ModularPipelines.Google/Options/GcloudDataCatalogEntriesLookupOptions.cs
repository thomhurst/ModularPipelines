using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "entries", "lookup")]
public record GcloudDataCatalogEntriesLookupOptions(
[property: PositionalArgument] string Resource
) : GcloudOptions;