using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "budgets", "delete")]
public record GcloudBillingBudgetsDeleteOptions(
[property: CliArgument] string Budget,
[property: CliArgument] string BillingAccount
) : GcloudOptions;