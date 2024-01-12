using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "entries", "unstar")]
public record GcloudDataCatalogEntriesUnstarOptions(
[property: PositionalArgument] string Entry,
[property: PositionalArgument] string EntryGroup,
[property: PositionalArgument] string Location
) : GcloudOptions;