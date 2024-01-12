using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "enable")]
public record GcloudIamServiceAccountsEnableOptions(
[property: PositionalArgument] string ServiceAccount
) : GcloudOptions;