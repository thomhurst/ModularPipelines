using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "entry-groups", "describe")]
public record GcloudDataCatalogEntryGroupsDescribeOptions(
[property: CliArgument] string EntryGroup,
[property: CliArgument] string Location
) : GcloudOptions;