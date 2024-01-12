using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "entry-groups", "list")]
public record GcloudDataCatalogEntryGroupsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;