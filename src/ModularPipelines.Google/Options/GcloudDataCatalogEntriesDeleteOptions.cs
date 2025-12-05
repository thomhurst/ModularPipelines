using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "entries", "delete")]
public record GcloudDataCatalogEntriesDeleteOptions(
[property: CliArgument] string Entry,
[property: CliArgument] string EntryGroup,
[property: CliArgument] string Location
) : GcloudOptions;