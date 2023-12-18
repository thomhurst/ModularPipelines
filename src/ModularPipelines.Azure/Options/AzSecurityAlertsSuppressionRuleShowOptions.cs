using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "alerts-suppression-rule", "show")]
public record AzSecurityAlertsSuppressionRuleShowOptions(
[property: CommandSwitch("--rule-name")] string RuleName
) : AzOptions
{
}