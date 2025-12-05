using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "disable")]
public record GcloudIamServiceAccountsDisableOptions(
[property: CliArgument] string ServiceAccount
) : GcloudOptions;