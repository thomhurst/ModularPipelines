using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tags", "list")]
public record GcloudDataCatalogTagsListOptions(
[property: CliOption("--entry")] string Entry,
[property: CliOption("--entry-group")] string EntryGroup,
[property: CliOption("--location")] string Location
) : GcloudOptions;