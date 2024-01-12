using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "entry-groups", "update")]
public record GcloudDataCatalogEntryGroupsUpdateOptions(
[property: PositionalArgument] string EntryGroup,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}