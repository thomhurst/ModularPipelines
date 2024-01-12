using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "content", "remove-iam-policy-binding")]
public record GcloudDataplexContentRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Content,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;