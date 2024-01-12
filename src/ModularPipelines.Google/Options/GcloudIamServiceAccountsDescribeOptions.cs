using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "describe")]
public record GcloudIamServiceAccountsDescribeOptions(
[property: PositionalArgument] string ServiceAccount
) : GcloudOptions;