using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "entries", "delete")]
public record GcloudDataCatalogEntriesDeleteOptions(
[property: PositionalArgument] string Entry,
[property: PositionalArgument] string EntryGroup,
[property: PositionalArgument] string Location
) : GcloudOptions;