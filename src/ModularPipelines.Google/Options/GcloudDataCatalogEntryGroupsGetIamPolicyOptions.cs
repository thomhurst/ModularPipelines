using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "entry-groups", "get-iam-policy")]
public record GcloudDataCatalogEntryGroupsGetIamPolicyOptions(
[property: CliArgument] string EntryGroup,
[property: CliArgument] string Location
) : GcloudOptions;