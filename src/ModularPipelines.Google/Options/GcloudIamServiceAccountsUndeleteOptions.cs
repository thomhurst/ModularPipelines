using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "undelete")]
public record GcloudIamServiceAccountsUndeleteOptions(
[property: CliArgument] string AccountId
) : GcloudOptions;