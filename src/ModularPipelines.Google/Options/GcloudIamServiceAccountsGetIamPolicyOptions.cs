using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "get-iam-policy")]
public record GcloudIamServiceAccountsGetIamPolicyOptions(
[property: PositionalArgument] string ServiceAccount
) : GcloudOptions;