using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "entry-groups", "add-iam-policy-binding")]
public record GcloudDataCatalogEntryGroupsAddIamPolicyBindingOptions(
[property: PositionalArgument] string EntryGroup,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;