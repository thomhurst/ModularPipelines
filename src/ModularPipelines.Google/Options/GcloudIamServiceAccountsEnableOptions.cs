using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "enable")]
public record GcloudIamServiceAccountsEnableOptions(
[property: CliArgument] string ServiceAccount
) : GcloudOptions;