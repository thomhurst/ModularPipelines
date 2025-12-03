using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "entry-groups", "create")]
public record GcloudDataCatalogEntryGroupsCreateOptions(
[property: CliArgument] string EntryGroup,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}