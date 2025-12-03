using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "budgets", "list")]
public record GcloudBillingBudgetsListOptions(
[property: CliOption("--billing-account")] string BillingAccount
) : GcloudOptions;