using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "environments", "remove-iam-policy-binding")]
public record GcloudDataplexEnvironmentsRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;