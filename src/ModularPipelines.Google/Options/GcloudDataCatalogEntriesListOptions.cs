using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "entries", "list")]
public record GcloudDataCatalogEntriesListOptions(
[property: CliOption("--entry-group")] string EntryGroup,
[property: CliOption("--location")] string Location
) : GcloudOptions;