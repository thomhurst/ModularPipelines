using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "entry-groups", "describe")]
public record GcloudDataCatalogEntryGroupsDescribeOptions(
[property: PositionalArgument] string EntryGroup,
[property: PositionalArgument] string Location
) : GcloudOptions;