using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "accounts", "describe")]
public record GcloudBillingAccountsDescribeOptions(
[property: PositionalArgument] string AccountId
) : GcloudOptions;