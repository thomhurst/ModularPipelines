using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "disable")]
public record GcloudIamServiceAccountsDisableOptions(
[property: PositionalArgument] string ServiceAccount
) : GcloudOptions;