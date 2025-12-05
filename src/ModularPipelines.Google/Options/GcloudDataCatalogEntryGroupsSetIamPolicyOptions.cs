using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "entry-groups", "set-iam-policy")]
public record GcloudDataCatalogEntryGroupsSetIamPolicyOptions(
[property: CliArgument] string EntryGroup,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;