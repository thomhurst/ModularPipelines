using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "set-iam-policy")]
public record GcloudIamServiceAccountsSetIamPolicyOptions(
[property: PositionalArgument] string ServiceAccount,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;