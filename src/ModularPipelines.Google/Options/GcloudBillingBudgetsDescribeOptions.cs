using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "budgets", "describe")]
public record GcloudBillingBudgetsDescribeOptions(
[property: PositionalArgument] string Budget,
[property: PositionalArgument] string BillingAccount
) : GcloudOptions;