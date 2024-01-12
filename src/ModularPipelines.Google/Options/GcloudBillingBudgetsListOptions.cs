using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "budgets", "list")]
public record GcloudBillingBudgetsListOptions(
[property: CommandSwitch("--billing-account")] string BillingAccount
) : GcloudOptions;