using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "alerts-suppression-rule", "delete")]
public record AzSecurityAlertsSuppressionRuleDeleteOptions(
[property: CliOption("--rule-name")] string RuleName
) : AzOptions;