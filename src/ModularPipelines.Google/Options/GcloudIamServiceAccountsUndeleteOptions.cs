using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "undelete")]
public record GcloudIamServiceAccountsUndeleteOptions(
[property: PositionalArgument] string AccountId
) : GcloudOptions;