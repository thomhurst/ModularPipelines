using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "budgets", "describe")]
public record GcloudBillingBudgetsDescribeOptions(
[property: CliArgument] string Budget,
[property: CliArgument] string BillingAccount
) : GcloudOptions;