using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "entries", "list")]
public record GcloudDataCatalogEntriesListOptions(
[property: CommandSwitch("--entry-group")] string EntryGroup,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;