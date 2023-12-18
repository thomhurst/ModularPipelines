using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "alerts-suppression-rule", "upsert_scope")]
public record AzSecurityAlertsSuppressionRuleUpsert_scopeOptions(
[property: CommandSwitch("--field")] string Field,
[property: CommandSwitch("--rule-name")] string RuleName
) : AzOptions
{
    [CommandSwitch("--any-of")]
    public string? AnyOf { get; set; }

    [CommandSwitch("--contains-substring")]
    public string? ContainsSubstring { get; set; }
}