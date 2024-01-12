using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "delete")]
public record GcloudIamServiceAccountsDeleteOptions(
[property: PositionalArgument] string ServiceAccount
) : GcloudOptions;