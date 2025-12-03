using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "alerts-suppression-rule", "delete_scope")]
public record AzSecurityAlertsSuppressionRuleDelete_scopeOptions(
[property: CliOption("--field")] string Field,
[property: CliOption("--rule-name")] string RuleName
) : AzOptions;