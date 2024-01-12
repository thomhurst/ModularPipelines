using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tags", "list")]
public record GcloudDataCatalogTagsListOptions(
[property: CommandSwitch("--entry")] string Entry,
[property: CommandSwitch("--entry-group")] string EntryGroup,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;