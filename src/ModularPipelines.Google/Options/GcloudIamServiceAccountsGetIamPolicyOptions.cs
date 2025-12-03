using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "get-iam-policy")]
public record GcloudIamServiceAccountsGetIamPolicyOptions(
[property: CliArgument] string ServiceAccount
) : GcloudOptions;