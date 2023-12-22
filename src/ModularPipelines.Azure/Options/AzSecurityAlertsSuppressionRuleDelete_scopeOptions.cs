using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "alerts-suppression-rule", "delete_scope")]
public record AzSecurityAlertsSuppressionRuleDelete_scopeOptions(
[property: CommandSwitch("--field")] string Field,
[property: CommandSwitch("--rule-name")] string RuleName
) : AzOptions;