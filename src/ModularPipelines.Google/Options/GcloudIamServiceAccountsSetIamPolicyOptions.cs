using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "set-iam-policy")]
public record GcloudIamServiceAccountsSetIamPolicyOptions(
[property: CliArgument] string ServiceAccount,
[property: CliArgument] string PolicyFile
) : GcloudOptions;