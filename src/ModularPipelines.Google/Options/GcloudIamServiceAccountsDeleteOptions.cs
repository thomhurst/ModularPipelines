using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "delete")]
public record GcloudIamServiceAccountsDeleteOptions(
[property: CliArgument] string ServiceAccount
) : GcloudOptions;