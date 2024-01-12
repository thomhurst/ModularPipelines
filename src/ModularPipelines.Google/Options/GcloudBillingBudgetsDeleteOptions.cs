using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "budgets", "delete")]
public record GcloudBillingBudgetsDeleteOptions(
[property: PositionalArgument] string Budget,
[property: PositionalArgument] string BillingAccount
) : GcloudOptions;